import { Injectable } from '@angular/core';
import { WebSocketService } from './web-socket.service';
import { Stock } from './models/stock';

@Injectable({
  providedIn: 'root',
})
export class StockService {
  public stocklist: Stock[];

  constructor(private websocketService: WebSocketService) {
    websocketService.subscribe('stocks');
    websocketService.subjects.get('stocks').subscribe((stocks) => {
      if (stocks !== null && stocks.action === 'update') {
        this.stocklist = new Array<Stock>();

        stocks.data.forEach((stock) => {
          this.stocklist.push({
            name: stock.Name,
            change: stock.HistoricPrice[1]
              ? 1 - stock.HistoricPrice[1].Price / stock.HistoricPrice[0].Price
              : 0,
            value: stock.HistoricPrice[0].Price,
          } as Stock);
        });
        console.log(this.stocklist);
      }
    });
  }
}
