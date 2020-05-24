import { Injectable } from '@angular/core';
import { WebSocketService } from './web-socket.service';

@Injectable({
  providedIn: 'root',
})
export class StockService {
  constructor(private websocketService: WebSocketService) {
    websocketService.subscribe('stocks');
    websocketService.subjects
      .get('stocks')
      .subscribe((stocks) => console.log(stocks));
  }
}
