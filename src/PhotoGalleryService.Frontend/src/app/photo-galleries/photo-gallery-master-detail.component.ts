import { PhotoGalleryAdd, PhotoGalleryDelete, PhotoGalleryEdit, photoGalleryActions } from "./photo-gallery.actions";
import { PhotoGallery } from "./photo-gallery.model";
import { PhotoGalleryService } from "./photo-gallery.service";

const template = require("./photo-gallery-master-detail.component.html");
const styles = require("./photo-gallery-master-detail.component.scss");

export class PhotoGalleryMasterDetailEmbedComponent extends HTMLElement {
    constructor(
        private _photoGallery: PhotoGalleryService = PhotoGalleryService.Instance
    ) {
        super();
        this.onPhotoGalleryAdd = this.onPhotoGalleryAdd.bind(this);
        this.onPhotoGalleryEdit = this.onPhotoGalleryEdit.bind(this);
        this.onPhotoGalleryDelete = this.onPhotoGalleryDelete.bind(this);
    }

    static get observedAttributes () {
        return [
            "photo-galleries"
        ];
    }

    connectedCallback() {
        this.innerHTML = `<style>${styles}</style> ${template}`;
        this._bind();
        this._setEventListeners();
    }

    private async _bind() {
        this.photoGalleryListElement.setAttribute("photo-galleries", JSON.stringify(this.photoGalleries));
    }

    private _setEventListeners() {
        this.addEventListener(photoGalleryActions.ADD, this.onPhotoGalleryAdd);
        this.addEventListener(photoGalleryActions.EDIT, this.onPhotoGalleryEdit);
        this.addEventListener(photoGalleryActions.DELETE, this.onPhotoGalleryDelete);
    }

    disconnectedCallback() {
        this.removeEventListener(photoGalleryActions.ADD, this.onPhotoGalleryAdd);
        this.removeEventListener(photoGalleryActions.EDIT, this.onPhotoGalleryEdit);
        this.removeEventListener(photoGalleryActions.DELETE, this.onPhotoGalleryDelete);
    }

    public onPhotoGalleryAdd(e) {

        const index = this.photoGalleries.findIndex(o => o.id == e.detail.photoGallery.id);
        const indexBaseOnUniqueIdentifier = this.photoGalleries.findIndex(o => o.name == e.detail.photoGallery.name);

        if (index > -1 && e.detail.photoGallery.id != null) {
            this.photoGalleries[index] = e.detail.photoGallery;
        } else if (indexBaseOnUniqueIdentifier > -1) {
            for (var i = 0; i < this.photoGalleries.length; ++i) {
                if (this.photoGalleries[i].name == e.detail.photoGallery.name)
                    this.photoGalleries[i] = e.detail.photoGallery;
            }
        } else {
            this.photoGalleries.push(e.detail.photoGallery);
        }
        
        this.photoGalleryListElement.setAttribute("photo-galleries", JSON.stringify(this.photoGalleries));
        this.photoGalleryEditElement.setAttribute("photo-gallery", JSON.stringify(new PhotoGallery()));
    }

    public onPhotoGalleryEdit(e) {
        this.photoGalleryEditElement.setAttribute("photo-gallery", JSON.stringify(e.detail.photoGallery));
    }

    public onPhotoGalleryDelete(e) {
        if (e.detail.photoGallery.Id != null && e.detail.photoGallery.Id != undefined) {
            this.photoGalleries.splice(this.photoGalleries.findIndex(o => o.id == e.detail.optionId), 1);
        } else {
            for (var i = 0; i < this.photoGalleries.length; ++i) {
                if (this.photoGalleries[i].name == e.detail.photoGallery.name)
                    this.photoGalleries.splice(i, 1);
            }
        }

        this.photoGalleryListElement.setAttribute("photo-galleries", JSON.stringify(this.photoGalleries));
        this.photoGalleryEditElement.setAttribute("photo-gallery", JSON.stringify(new PhotoGallery()));
    }

    attributeChangedCallback (name, oldValue, newValue) {
        switch (name) {
            case "photo-galleries":
                this.photoGalleries = JSON.parse(newValue);

                if (this.parentNode)
                    this.connectedCallback();

                break;
        }
    }

    public get value(): Array<PhotoGallery> { return this.photoGalleries; }

    private photoGalleries: Array<PhotoGallery> = [];
    public photoGallery: PhotoGallery = <PhotoGallery>{};
    public get photoGalleryEditElement(): HTMLElement { return this.querySelector("ce-photo-gallery-edit-embed") as HTMLElement; }
    public get photoGalleryListElement(): HTMLElement { return this.querySelector("ce-photo-gallery-list-embed") as HTMLElement; }
}

customElements.define(`ce-photo-gallery-master-detail`,PhotoGalleryMasterDetailEmbedComponent);
