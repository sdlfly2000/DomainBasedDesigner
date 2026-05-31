import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { AppFooterService } from '../service/app.footer.service';

@Component({
    standalone: true,
    selector: 'app-footer',
    template: `<div class="layout-footer">Auth Service [{{Version}}]</div>`
})
export class AppFooter implements OnInit {
    private _version: string = '';

    constructor(
        private appFooterService: AppFooterService,
        private cdr: ChangeDetectorRef) {
    }

    ngOnInit(): void {
        this.appFooterService.GetVersion().subscribe(ver => {
            this._version = ver;
            this.cdr.markForCheck();
        });
    }

    get Version(): string {
        return this._version;
    }
}
