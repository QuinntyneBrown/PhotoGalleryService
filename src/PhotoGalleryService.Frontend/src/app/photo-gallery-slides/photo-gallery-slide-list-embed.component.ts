import { PhotoGallerySlide } from "./photo-gallery-slide.model";

const template = require("./photo-gallery-slide-list-embed.component.html");
const styles = require("./photo-gallery-slide-list-embed.component.scss");

export class PhotoGallerySlideListEmbedComponent extends HTMLElement {
    constructor(
        private _document: Document = document
    ) {
        super();
    }


    static get observedAttributes() {
        return [
            "photo-gallery-slides"
        ];
    }

    connectedCallback() {
        this.innerHTML = `<style>${styles}</style> ${template}`;
        this._bind();
    }

    private async _bind() {        
        for (let i = 0; i < this.photoGallerySlides.length; i++) {
            let el = this._document.createElement(`ce-photo-gallery-slide-item-embed`);
            el.setAttribute("entity", JSON.stringify(this.photoGallerySlides[i]));
            this.appendChild(el);
        }    
    }

    photoGallerySlides:Array<PhotoGallerySlide> = [];

    attributeChangedCallback(name, oldValue, newValue) {
        switch (name) {
            case "photo-gallery-slides":
                this.photoGallerySlides = JSON.parse(newValue);
                if (this.parentElement)
                    this.connectedCallback();
                break;
        }
    }
}

customElements.define("ce-photo-gallery-slide-list-embed", PhotoGallerySlideListEmbedComponent);
