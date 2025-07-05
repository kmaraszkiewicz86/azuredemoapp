import { SendJsonModels } from "./features/send-json/send-json.models";

export interface AppState {
  jsonData: SendJsonModels;
  loading: boolean;
  error?: string;
}
