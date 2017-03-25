import { PhotoGallery } from "./photo-gallery.model";

const template = require("./photo-gallery-list-embed.component.html");
const styles = require("./photo-gallery-list-embed.component.scss");

export class PhotoGalleryListEmbedComponent extends HTMLElement {
    constructor(
        private _document: Document = document
    ) {
        super();
    }


    static get observedAttributes() {
        return [
            "photo-galleries"
        ];
    }

    connectedCallback() {
        this.innerHTML = `<style>${styles}</style> ${template}`;
        this._bind();
    }

    private async _bind() {        
        for (let i = 0; i < this.photoGallerys.length; i++) {
            let el = this._document.createElement(`ce-photo-gallery-item-embed`);
            el.setAttribute("entity", JSON.stringify(this.photoGallerys[i]));
            this.appendChild(el);
        }    
    }

    photoGallerys:Array<PhotoGallery> = [];

    attributeChangedCallback(name, oldValue, newValue) {
        switch (name) {
            case "photo-galleries":
                this.photoGallerys = JSON.parse(newValue);
                if (this.parentElement)
                    this.connectedCallback();
                break;
        }
    }
}

customElements.define("ce-photo-gallery-list-embed", PhotoGalleryListEmbedComponent);
