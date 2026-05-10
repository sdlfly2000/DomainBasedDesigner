import { Component } from '@angular/core';
import { AppFooterService } from '../service/app.footer.service';

@Component({
    standalone: true,
    selector: 'app-footer',
    template: `<div class="layout-footer">Auth Service [{{Version}}]</div>`
})
export class AppFooter {
    private _version: string = '';

    constructor(private appFooterService: AppFooterService) {
        this.appFooterService.GetVersion().subscribe(ver => this._version = ver);
    }

    get Version(): string {
        return this._version;
    }
}
