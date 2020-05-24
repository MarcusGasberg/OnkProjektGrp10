export interface WsMessage {
  topic: string;
  data?: any;
  action?: string;
}
