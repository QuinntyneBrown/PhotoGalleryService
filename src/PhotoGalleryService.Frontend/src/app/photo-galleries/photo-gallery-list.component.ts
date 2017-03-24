import { PhotoGallery } from "./photo-gallery.model";
import { PhotoGalleryService } from "./photo-gallery.service";

const template = require("./photo-gallery-list.component.html");
const styles = require("./photo-gallery-list.component.scss");

export class PhotoGalleryListComponent extends HTMLElement {
    constructor(
        private _document: Document = document,
        private _photoGalleryService: PhotoGalleryService = PhotoGalleryService.Instance) {
        super();
    }

    connectedCallback() {
        this.innerHTML = `<style>${styles}</style> ${template}`;
        this._bind();
    }

    private async _bind() {
        const photoGallerys: Array<PhotoGallery> = await this._photoGalleryService.get();
        for (var i = 0; i < photoGallerys.length; i++) {
            let el = this._document.createElement(`ce-photo-gallery-item`);
            el.setAttribute("entity", JSON.stringify(photoGallerys[i]));
            this.appendChild(el);
        }    
    }
}

customElements.define("ce-photo-gallery-list", PhotoGalleryListComponent);
