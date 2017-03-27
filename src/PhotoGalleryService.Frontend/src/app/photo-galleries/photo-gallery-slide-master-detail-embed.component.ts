import { PhotoGallerySlideAdd, PhotoGallerySlideDelete, PhotoGallerySlideEdit, photoGallerySlideActions } from "./photo-gallery-slide.actions";
import { PhotoGallerySlide } from "./photo-gallery-slide.model";

const template = require("./photo-gallery-slide-master-detail-embed.component.html");
const styles = require("./photo-gallery-slide-master-detail-embed.component.scss");

export class PhotoGallerySlideMasterDetailEmbedComponent extends HTMLElement {
    constructor() {
        super();
        this.onPhotoGallerySlideAdd = this.onPhotoGallerySlideAdd.bind(this);
        this.onPhotoGallerySlideEdit = this.onPhotoGallerySlideEdit.bind(this);
        this.onPhotoGallerySlideDelete = this.onPhotoGallerySlideDelete.bind(this);
    }

    static get observedAttributes () {
        return [
            "photo-gallery-slides"
        ];
    }

    connectedCallback() {
        this.innerHTML = `<style>${styles}</style> ${template}`;
        this._bind();
        this._setEventListeners();
    }

    private async _bind() {
        this.photoGallerySlideListElement.setAttribute("photo-gallery-slides", JSON.stringify(this.photoGallerySlides));
    }

    private _setEventListeners() {
        this.addEventListener(photoGallerySlideActions.ADD, this.onPhotoGallerySlideAdd);
        this.addEventListener(photoGallerySlideActions.EDIT, this.onPhotoGallerySlideEdit);
        this.addEventListener(photoGallerySlideActions.DELETE, this.onPhotoGallerySlideDelete);
    }

    disconnectedCallback() {
        this.removeEventListener(photoGallerySlideActions.ADD, this.onPhotoGallerySlideAdd);
        this.removeEventListener(photoGallerySlideActions.EDIT, this.onPhotoGallerySlideEdit);
        this.removeEventListener(photoGallerySlideActions.DELETE, this.onPhotoGallerySlideDelete);
    }

    public onPhotoGallerySlideAdd(e) {

        //const index = this.photoGallerySlides.findIndex(o => o.id == e.detail.photoGallerySlide.id);
        //const indexBaseOnUniqueIdentifier = this.photoGallerySlides.findIndex(o => o.name == e.detail.photoGallerySlide.name);

        //if (index > -1 && e.detail.photoGallerySlide.id != null) {
        //    this.photoGallerySlides[index] = e.detail.photoGallerySlide;
        //} else if (indexBaseOnUniqueIdentifier > -1) {
        //    for (var i = 0; i < this.photoGallerySlides.length; ++i) {
        //        if (this.photoGallerySlides[i].name == e.detail.photoGallerySlide.name)
        //            this.photoGallerySlides[i] = e.detail.photoGallerySlide;
        //    }
        //} else {
        //    this.photoGallerySlides.push(e.detail.photoGallerySlide);
        //}
        
        this.photoGallerySlideListElement.setAttribute("photo-gallery-slides", JSON.stringify(this.photoGallerySlides));
        this.photoGallerySlideEditElement.setAttribute("photo-gallery-slide", JSON.stringify(new PhotoGallerySlide()));
    }

    public onPhotoGallerySlideEdit(e) {
        this.photoGallerySlideEditElement.setAttribute("photo-gallery-slide", JSON.stringify(e.detail.photoGallerySlide));
    }

    public onPhotoGallerySlideDelete(e) {
        if (e.detail.photoGallerySlide.Id != null && e.detail.photoGallerySlide.Id != undefined) {
            this.photoGallerySlides.splice(this.photoGallerySlides.findIndex(o => o.id == e.detail.optionId), 1);
        } else {
            for (var i = 0; i < this.photoGallerySlides.length; ++i) {
                if (this.photoGallerySlides[i].name == e.detail.photoGallerySlide.name)
                    this.photoGallerySlides.splice(i, 1);
            }
        }

        this.photoGallerySlideListElement.setAttribute("photo-gallery-slides", JSON.stringify(this.photoGallerySlides));
        this.photoGallerySlideEditElement.setAttribute("photo-gallery-slide", JSON.stringify(new PhotoGallerySlide()));
    }

    attributeChangedCallback (name, oldValue, newValue) {
        switch (name) {
            case "photo-gallery-slides":
                this.photoGallerySlides = JSON.parse(newValue);

                if (this.parentNode)
                    this.connectedCallback();

                break;
        }
    }

    public get value(): Array<PhotoGallerySlide> { return this.photoGallerySlides; }

    private photoGallerySlides: Array<PhotoGallerySlide> = [];
    public photoGallerySlide: PhotoGallerySlide = <PhotoGallerySlide>{};
    public get photoGallerySlideEditElement(): HTMLElement { return this.querySelector("ce-photo-gallery-slide-edit-embed") as HTMLElement; }
    public get photoGallerySlideListElement(): HTMLElement { return this.querySelector("ce-photo-gallery-slide-list-embed") as HTMLElement; }
}

customElements.define(`ce-photo-gallery-slide-master-detail-embed`,PhotoGallerySlideMasterDetailEmbedComponent);
