import { PhotoGallery } from "./photo-gallery.model";
import { EditorComponent } from "../shared";
import {  PhotoGalleryDelete, PhotoGalleryEdit, PhotoGalleryAdd } from "./photo-gallery.actions";

const template = require("./photo-gallery-edit-embed.component.html");
const styles = require("./photo-gallery-edit-embed.component.scss");

export class PhotoGalleryEditEmbedComponent extends HTMLElement {
    constructor() {
        super();
        this.onSave = this.onSave.bind(this);
        this.onDelete = this.onDelete.bind(this);
        this.onCreate = this.onCreate.bind(this);
    }

    static get observedAttributes() {
        return [
            "photo-gallery",
            "photo-gallery-id"
        ];
    }
    
    connectedCallback() {        
        this.innerHTML = `<style>${styles}</style> ${template}`; 
        this._bind();
        this._setEventListeners();
    }
    
    private async _bind() {
        this._titleElement.textContent = this.photoGallery ? "Edit Photo Gallery": "Create Photo Gallery";

        if (this.photoGallery) {                
            this._nameInputElement.value = this.photoGallery.name;  
        } else {
            this._deleteButtonElement.style.display = "none";
        }     
    }

    private _setEventListeners() {
        this._saveButtonElement.addEventListener("click", this.onSave);
        this._deleteButtonElement.addEventListener("click", this.onDelete);
        this._createButtonElement.addEventListener("click", this.onCreate);
    }

    private disconnectedCallback() {
        this._saveButtonElement.removeEventListener("click", this.onSave);
        this._deleteButtonElement.removeEventListener("click", this.onDelete);
        this._createButtonElement.removeEventListener("click", this.onCreate);
    }

    public onSave() {
        const photoGallery = {
            id: this.photoGallery != null ? this.photoGallery.id : null,
            name: this._nameInputElement.value
        } as PhotoGallery;
        
        this.dispatchEvent(new PhotoGalleryAdd(photoGallery));            
    }

    public onCreate() {        
        this.dispatchEvent(new PhotoGalleryEdit(new PhotoGallery()));            
    }

    public onDelete() {        
        const photoGallery = {
            id: this.photoGallery != null ? this.photoGallery.id : null,
            name: this._nameInputElement.value
        } as PhotoGallery;

        this.dispatchEvent(new PhotoGalleryDelete(photoGallery));         
    }

    attributeChangedCallback(name, oldValue, newValue) {
        switch (name) {
            case "photo-gallery-id":
                this.photoGalleryId = newValue;
                break;
            case "photo-gallery":
                this.photoGallery = JSON.parse(newValue);
                if (this.parentNode) {
                    this.photoGalleryId = this.photoGallery.id;
                    this._nameInputElement.value = this.photoGallery.name != undefined ? this.photoGallery.name : "";
                    this._titleElement.textContent = this.photoGalleryId ? "Edit PhotoGallery" : "Create PhotoGallery";
                }
                break;
        }           
    }

    public photoGalleryId: any;
    
	public photoGallery: PhotoGallery;
    
    private get _createButtonElement(): HTMLElement { return this.querySelector(".photo-gallery-create") as HTMLElement; }
    
	private get _titleElement(): HTMLElement { return this.querySelector("h2") as HTMLElement; }
    
	private get _saveButtonElement(): HTMLElement { return this.querySelector(".save-button") as HTMLElement };
    
	private get _deleteButtonElement(): HTMLElement { return this.querySelector(".delete-button") as HTMLElement };
    
	private get _nameInputElement(): HTMLInputElement { return this.querySelector(".photo-gallery-name") as HTMLInputElement;}
}

customElements.define(`ce-photo-gallery-edit-embed`,PhotoGalleryEditEmbedComponent);
