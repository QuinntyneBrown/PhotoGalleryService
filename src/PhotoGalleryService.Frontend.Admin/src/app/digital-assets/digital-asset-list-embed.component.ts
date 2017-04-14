import { DigitalAsset } from "./digital-asset.model";

const template = require("./digital-asset-list-embed.component.html");
const styles = require("./digital-asset-list-embed.component.scss");

export class DigitalAssetListEmbedComponent extends HTMLElement {
    constructor(
        private _document: Document = document
    ) {
        super();
    }


    static get observedAttributes() {
        return [
            "digital-assets"
        ];
    }

    connectedCallback() {
        this.innerHTML = `<style>${styles}</style> ${template}`;
        this._bind();
    }

    private async _bind() {        
        for (let i = 0; i < this.digitalAssets.length; i++) {
            let el = this._document.createElement(`ce-digital-asset-item-embed`);
            el.setAttribute("entity", JSON.stringify(this.digitalAssets[i]));
            this.appendChild(el);
        }    
    }

    digitalAssets:Array<DigitalAsset> = [];

    attributeChangedCallback(name, oldValue, newValue) {
        switch (name) {
            case "digital-assets":                
                this.digitalAssets = JSON.parse(newValue);
                if (this.parentNode) {
                    this.connectedCallback();
                }
                break;
        }
    }
}

customElements.define("ce-digital-asset-list-embed", DigitalAssetListEmbedComponent);
