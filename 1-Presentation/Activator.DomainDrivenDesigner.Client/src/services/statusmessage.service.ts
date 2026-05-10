import { Injectable } from "@angular/core";
import { Observable, Subject } from "rxjs";

@Injectable({
  providedIn: "root"
})
export class StatusMessageService {

  private statusMessage: Subject<StatusMessageModel>;

  constructor() {
    this.statusMessage = new Subject<StatusMessageModel>();
  }

  set StatusMessage(StatusMessageModel: StatusMessageModel) {
    this.statusMessage.next(StatusMessageModel);
  }

  get StatusMessage(): Observable<StatusMessageModel> {
    return this.statusMessage;
  }
}

export enum EnumInfoSeverity {
    Success = "success",
    Info = "info",
    Warn = "warn",
    Error = "error"
}

export class StatusMessageModel {

    Message: string | undefined
    InfoSeverity: EnumInfoSeverity | undefined

    constructor(message: string, level: EnumInfoSeverity) {
        this.Message = message;
        this.InfoSeverity = level;
    }
}
