import { Injectable } from "@angular/core";
import { ActivatedRoute } from "@angular/router";


@Injectable({
  providedIn: "root"
})
export class ContextService {
    private projectId: string | undefined;
    private projectName: string | undefined;
      
    constructor() {
    }

    get ProjectId(): string | undefined {
        return this.projectId;
     }

    set ProjectId(projectId: string | undefined) {
        this.projectId = projectId;
    }

    get ProjectName(): string | undefined {
        return this.projectName;
    }

    set ProjectName(projectName: string | undefined) {
        this.projectName = projectName;
    }
}
