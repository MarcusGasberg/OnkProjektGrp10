import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Subject, Observable, Observer, BehaviorSubject } from 'rxjs';
import { webSocket, WebSocketSubject } from 'rxjs/webSocket';
import { OidcSecurityService } from 'angular-auth-oidc-client';

@Injectable({
  providedIn: 'root',
})
export class WebSocketService {
  private messages = new Subject<any>();
  private ws: WebSocketSubject<unknown>;

  public subjects = new Map<string, BehaviorSubject<any>>();

  constructor(private oidc: OidcSecurityService) {
    this.createSocket();
  }

  private createSocket() {
    this.ws = webSocket(
      environment.websocketUrl + `?access_token=${this.oidc.getToken()}`
    );
    this.ws.subscribe(this.onMessage, this.onError, this.onComplete);
  }

  private onMessage = (payload) => {
    if (this.subjects.has(payload.topic)) {
      this.subjects.get(payload.topic).next(payload);
    } else {
      this.subjects.set(payload.topic, new BehaviorSubject(payload));
    }
  };

  private onError = (payload) => {
    this.messages.error(payload);
  };

  private onComplete() {
    this.messages.complete();
    this.createSocket();
  }

  public subscribe(topic: string) {
    if (!this.subjects.get(topic)) {
      this.subjects.set(topic, new BehaviorSubject(null));
      const msg = {
        topic,
        action: 'subscribe',
      };
      this.ws.next(msg);
    }
  }

  public sendMessage(topic: string, data?: object, action?: string) {
    const msg = {
      topic,
      data: JSON.stringify(data),
      action,
    };
    this.ws.next(msg);
  }
}
