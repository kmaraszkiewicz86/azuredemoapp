import { JsonModel } from "./features/send-json/send-json.models";

export interface AppState {
  jsonData: JsonModel;
  loading: boolean;
  error?: string;
}
