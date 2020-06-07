import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Subject, Observable, Observer, BehaviorSubject } from 'rxjs';
import { webSocket, WebSocketSubject } from 'rxjs/webSocket';

@Injectable({
  providedIn: 'root',
})
export class WebSocketService {
  private messages = new Subject<any>();
  private ws: WebSocketSubject<unknown>;

  public subjects = new Map<string, BehaviorSubject<any>>();

  constructor() {
    this.createSocket();
  }

  private createSocket() {
    this.ws = webSocket(environment.websocketUrl);
    //ws://${window.location.hostname}/stockmarketws
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
