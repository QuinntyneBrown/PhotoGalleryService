import { PhotoAdd, PhotoDelete, PhotoEdit, photoActions } from "./photo.actions";
import { Photo } from "./photo.model";
import { PhotoService } from "./photo.service";
import { PhotoGalleryService, PhotoGallery } from "../photo-galleries";

const template = require("./photo-master-detail.component.html");
const styles = require("./photo-master-detail.component.scss");

export class PhotoMasterDetailComponent extends HTMLElement {
    constructor(
        private _photoService: PhotoService = PhotoService.Instance,
        private _photoGalleryService: PhotoGalleryService = PhotoGalleryService.Instance
	) {
        super();
        this.onPhotoAdd = this.onPhotoAdd.bind(this);
        this.onPhotoEdit = this.onPhotoEdit.bind(this);
        this.onPhotoDelete = this.onPhotoDelete.bind(this);
    }

    static get observedAttributes () {
        return [
            "photos"
        ];
    }

    connectedCallback() {
        this.innerHTML = `<style>${styles}</style> ${template}`;
        this._bind();
        this._setEventListeners();
    }

    private async _bind() {        
        this.photoGalleries = await this._photoGalleryService.get();
        this.photos = this.photoGalleries[0].photos;

        for (let i = 0; i < this.photoGalleries.length; i++) {
            let option = document.createElement("option");
            option.textContent = this.photoGalleries[i].name;
            option.value = this.photoGalleries[i].id;
            this.photoGallerySelectElement.appendChild(option);
        }

        this.photoGalleryId = this.photoGalleries[0].id;

        this.photoListElement.setAttribute("photos", JSON.stringify(this.photos));
    }

    private _setEventListeners() {
        this.addEventListener(photoActions.ADD, this.onPhotoAdd);
        this.addEventListener(photoActions.EDIT, this.onPhotoEdit);
        this.addEventListener(photoActions.DELETE, this.onPhotoDelete);
    }

    disconnectedCallback() {
        this.removeEventListener(photoActions.ADD, this.onPhotoAdd);
        this.removeEventListener(photoActions.EDIT, this.onPhotoEdit);
        this.removeEventListener(photoActions.DELETE, this.onPhotoDelete);
    }

    public async onPhotoAdd(e) {

        e.detail.photo.photoGalleryId = this.photoGalleryId;

        await this._photoService.add(e.detail.photo);
        this.photos = await this._photoService.get();
        
        this.photoListElement.setAttribute("photos", JSON.stringify(this.photos));
        this.photoEditElement.setAttribute("photo", JSON.stringify(new Photo()));
    }

    public onPhotoEdit(e) {
        this.photoEditElement.setAttribute("photo", JSON.stringify(e.detail.photo));
    }

    public async onPhotoDelete(e) {

        await this._photoService.remove(e.detail.photo.id);
        this.photos = await this._photoService.get();
        
        this.photoListElement.setAttribute("photos", JSON.stringify(this.photos));
        this.photoEditElement.setAttribute("photo", JSON.stringify(new Photo()));
    }

    attributeChangedCallback (name, oldValue, newValue) {
        switch (name) {
            case "photos":
                this.photos = JSON.parse(newValue);

                if (this.parentNode)
                    this.connectedCallback();

                break;
        }
    }

    public get value(): Array<Photo> { return this.photos; }
    
    private photos: Array<Photo> = [];

    public photoGalleryId: any;

    private photoGalleries: Array<PhotoGallery> = [];

    public photo: Photo = <Photo>{};

    public get photoGallerySelectElement(): HTMLSelectElement { return this.querySelector("select"); }

    public get photoEditElement(): HTMLElement { return this.querySelector("ce-photo-edit-embed") as HTMLElement; }

    public get photoListElement(): HTMLElement { return this.querySelector("ce-photo-paginated-list-embed") as HTMLElement; }
}

customElements.define(`ce-photo-master-detail`,PhotoMasterDetailComponent);
