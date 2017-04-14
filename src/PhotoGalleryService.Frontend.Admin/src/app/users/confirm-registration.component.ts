const template = require("./confirm-registration.component.html");
const styles = require("./confirm-registration.component.scss");

export class ConfirmRegistrationComponent extends HTMLElement {
    constructor() {
        super();
    }

    static get observedAttributes () {
        return [
            "token"
        ];
    }

    connectedCallback() {
        this.innerHTML = `<style>${styles}</style> ${template}`;
        this._bind();
        this._setEventListeners();
    }

    private async _bind() {

    }

    private _setEventListeners() {

    }

    disconnectedCallback() {

    }

    private token: string;

    attributeChangedCallback (name, oldValue, newValue) {
        switch (name) {
            case "token":
                this.token = newValue;
                break;
        }
    }
}

customElements.define(`ce-confirm-registration`,ConfirmRegistrationComponent);
