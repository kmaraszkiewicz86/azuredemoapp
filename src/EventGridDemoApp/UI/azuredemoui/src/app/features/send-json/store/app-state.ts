import { GetJsonState } from "./reducers/send-json-get.reducer";
import { SendJsonState } from "./reducers/send-json-send.reducer";

export interface SendJsonAppState {
  send: SendJsonState;
  get: GetJsonState;
}
