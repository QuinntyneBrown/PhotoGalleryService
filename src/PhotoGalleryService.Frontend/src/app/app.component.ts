import { AppRouterOutletComponent } from "./app-router-outlet.component";
import { Storage, TENANT_KEY } from "./utilities";

const template = require("./app.component.html");
const styles = require("./app.component.scss");

export class AppComponent extends HTMLElement {
    constructor(
        private _storage: Storage = Storage.Instance
    ) {
        super();
    }

    connectedCallback() {
        this._storage.put({ name: TENANT_KEY, value: `e1c91360-edbf-44e2-b31e-ec481f6e1d53` });
        this.innerHTML = `<style>${styles}</style>${template}`;
        new AppRouterOutletComponent(this.querySelector(".router-outlet"));
    }
}

customElements.define(`ce-app`, AppComponent);