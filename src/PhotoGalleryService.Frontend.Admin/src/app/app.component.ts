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
        this._storage.put({ name: TENANT_KEY, value: `50848e1d-f3ec-486a-b25c-7f6cf1ef7c93` });
        this.innerHTML = `<style>${styles}</style>${template}`;
        new AppRouterOutletComponent(this.querySelector(".router-outlet"));
    }
}

customElements.define(`ce-app`, AppComponent);