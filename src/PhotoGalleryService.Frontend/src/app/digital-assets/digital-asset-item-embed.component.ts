import { DigitalAsset } from "./digital-asset.model";
import { DigitalAssetDelete } from "./digital-asset.actions";

const template = require("./digital-asset-item-embed.component.html");
const styles = require("./digital-asset-item-embed.component.scss");

export class DigitalAssetItemEmbedComponent extends HTMLElement {
    constructor() {
        super();

        this._onDeleteClick = this._onDeleteClick.bind(this);
        this._onEditClick = this._onEditClick.bind(this);
        this._onViewClick = this._onViewClick.bind(this);
    }

    static get observedAttributes() {
        return ["entity"];
    }
    
    connectedCallback() {        
        this.innerHTML = `<style>${styles}</style> ${template}`;
        this._bind();
        this._setEventListeners();
    }

    disconnectedCallback() {
        this._deleteLinkElement.removeEventListener("click", this._onDeleteClick);
        this._editLinkElement.removeEventListener("click", this._onEditClick);
        this._viewLinkElement.removeEventListener("click", this._onViewClick);
    }

    private _bind() {
        this._digitalAssetUrlElement.textContent = this.entity.digitalAssetUrl;
    }

    private _setEventListeners() {
        this._deleteLinkElement.addEventListener("click", this._onDeleteClick);
        this._editLinkElement.addEventListener("click", this._onEditClick);
        this._viewLinkElement.addEventListener("click", this._onViewClick);
    }

    private async _onDeleteClick(e:Event) {
        const digitalAsset = {
            id: this.entity.id,
            digitalAssetUrl: this.entity.digitalAssetUrl
        } as DigitalAsset;

        this.dispatchEvent(new DigitalAssetDelete(digitalAsset)); 
    }

    private _onEditClick() {

    }

    private _onViewClick() {

    }
    
    attributeChangedCallback(name, oldValue, newValue) {
        switch (name) {
            case "entity":
                this.entity = JSON.parse(newValue);
                break;
        }        
    }

    private get _digitalAssetUrlElement() { return this.querySelector("p") as HTMLElement; }
    private get _deleteLinkElement() { return this.querySelector(".entity-item-delete") as HTMLElement; }
    private get _editLinkElement() { return this.querySelector(".entity-item-edit") as HTMLElement; }
    private get _viewLinkElement() { return this.querySelector(".entity-item-view") as HTMLElement; }
    public entity: DigitalAsset;
}

customElements.define(`ce-digital-asset-item-embed`,DigitalAssetItemEmbedComponent);
