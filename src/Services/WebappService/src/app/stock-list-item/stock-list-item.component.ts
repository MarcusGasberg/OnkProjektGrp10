import { Component, OnInit } from '@angular/core';
import { faArrowRight } from '@fortawesome/free-solid-svg-icons';

@Component( {
  selector: 'app-stock-list-item',
  templateUrl: './stock-list-item.component.html',
  styleUrls: [ './stock-list-item.component.scss' ]
} )
export class StockListItemComponent implements OnInit {

  public stockName: string = 'Stock';
  public stockValueChange: number = 5;
  public icon = faArrowRight;

  public get rotation (): number {
    if ( this.stockValueChange > 0 ) {
      return 315;
    }
    else {
      return 45;
    }
  }


  constructor () { }

  ngOnInit (): void {
  }

}
