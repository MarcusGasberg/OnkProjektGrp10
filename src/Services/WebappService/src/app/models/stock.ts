import { Seller } from './seller';
import { StockPrice } from './stockPrice';

export interface Stock {
  name: string;
  value: number;
  change: number;
  id: string;
  owned?: number;
  historicPrice: StockPrice[];
  seller: Seller[];
}
