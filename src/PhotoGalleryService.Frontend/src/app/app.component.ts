import { AppRouterOutletComponent } from "./app-router-outlet.component";

const template = require("./app.component.html");
const styles = require("./app.component.scss");

export class AppComponent extends HTMLElement {
    constructor() {
        super();
    }

    connectedCallback() {
        this.innerHTML = `<style>${styles}</style>${template}`;
        new AppRouterOutletComponent(this.querySelector(".router-outlet"));
    }
}

customElements.define(`ce-app`, AppComponent);