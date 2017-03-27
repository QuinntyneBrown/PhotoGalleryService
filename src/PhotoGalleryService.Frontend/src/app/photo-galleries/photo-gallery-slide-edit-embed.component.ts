import { PhotoGallerySlide } from "./photo-gallery-slide.model";
import { EditorComponent } from "../shared";
import {  PhotoGallerySlideDelete, PhotoGallerySlideEdit, PhotoGallerySlideAdd } from "./photo-gallery-slide.actions";

const template = require("./photo-gallery-slide-edit-embed.component.html");
const styles = require("./photo-gallery-slide-edit-embed.component.scss");

export class PhotoGallerySlideEditEmbedComponent extends HTMLElement {
    constructor() {
        super();
        this.onSave = this.onSave.bind(this);
        this.onDelete = this.onDelete.bind(this);
    }

    static get observedAttributes() {
        return [
            "photo-gallery-slide",
            "photo-gallery-slide-id"
        ];
    }
    
    connectedCallback() {        
        this.innerHTML = `<style>${styles}</style> ${template}`; 
        this._bind();
        this._setEventListeners();
    }
    
    private async _bind() {
        this._titleElement.textContent = this.photoGallerySlide ? "Edit Photo Gallery Slide": "Create Photo Gallery Slide";

        if (this.photoGallerySlide) {                
            this._nameInputElement.value = this.photoGallerySlide.name;  
        } else {
            this._deleteButtonElement.style.display = "none";
        }     
    }

    private _setEventListeners() {
        this._saveButtonElement.addEventListener("click", this.onSave);
        this._deleteButtonElement.addEventListener("click", this.onDelete);
    }

    private disconnectedCallback() {
        this._saveButtonElement.removeEventListener("click", this.onSave);
        this._deleteButtonElement.removeEventListener("click", this.onDelete);
    }

    public onSave() {
        const photoGallerySlide = {
            id: this.photoGallerySlide != null ? this.photoGallerySlide.id : null,
            name: this._nameInputElement.value
        } as PhotoGallerySlide;
        
        this.dispatchEvent(new PhotoGallerySlideAdd(photoGallerySlide));            
    }

    public onDelete() {        
        const photoGallerySlide = {
            id: this.photoGallerySlide != null ? this.photoGallerySlide.id : null,
            name: this._nameInputElement.value
        } as PhotoGallerySlide;

        this.dispatchEvent(new PhotoGallerySlideDelete(photoGallerySlide));         
    }

    attributeChangedCallback(name, oldValue, newValue) {
        switch (name) {
            case "photo-gallery-slide-id":
                this.photoGallerySlideId = newValue;
                break;
            case "photoGallerySlide":
                this.photoGallerySlide = JSON.parse(newValue);
                if (this.parentNode) {
                    this.photoGallerySlideId = this.photoGallerySlide.id;
                    this._nameInputElement.value = this.photoGallerySlide.name != undefined ? this.photoGallerySlide.name : "";
                    this._titleElement.textContent = this.photoGallerySlideId ? "Edit PhotoGallerySlide" : "Create PhotoGallerySlide";
                }
                break;
        }           
    }

    public photoGallerySlideId: any;
    public photoGallerySlide: PhotoGallerySlide;
    
    private get _titleElement(): HTMLElement { return this.querySelector("h2") as HTMLElement; }
    private get _saveButtonElement(): HTMLElement { return this.querySelector(".save-button") as HTMLElement };
    private get _deleteButtonElement(): HTMLElement { return this.querySelector(".delete-button") as HTMLElement };
    private get _nameInputElement(): HTMLInputElement { return this.querySelector(".photo-gallery-slide-name") as HTMLInputElement;}
}

customElements.define(`ce-photo-gallery-slide-edit-embed`,PhotoGallerySlideEditEmbedComponent);
