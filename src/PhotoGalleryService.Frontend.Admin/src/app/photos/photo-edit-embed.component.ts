import { Photo } from "./photo.model";
import { EditorComponent } from "../shared";
import { PhotoDelete, PhotoEdit, PhotoAdd } from "./photo.actions";

const template = require("./photo-edit-embed.component.html");
const styles = require("./photo-edit-embed.component.scss");

export class PhotoEditEmbedComponent extends HTMLElement {
    constructor() {
        super();
        this.onSave = this.onSave.bind(this);
        this.onDelete = this.onDelete.bind(this);
        this.onCreate = this.onCreate.bind(this);
    }

    static get observedAttributes() {
        return [
            "photo",
            "photo-id"
        ];
    }
    
    connectedCallback() {        
        this.innerHTML = `<style>${styles}</style> ${template}`; 
        this._bind();
        this._setEventListeners();
    }
    
    private async _bind() {

        this.descriptionEditor = new EditorComponent(this._descriptionInputElement);

        this._titleElement.textContent = this.photo ? "Edit Photo": "Create Photo";

        if (this.photo) {                
            this._nameInputElement.value = this.photo.name;  
            this._orderIndexInputElement.value = this.photo.orderIndex != undefined ? this.photo.orderIndex : "";
            this._imageUrlInputElement.value = this.photo.imageUrl != undefined ? this.photo.imageUrl : "";
            this.descriptionEditor.setHTML(this.photo.description != undefined ? this.photo.description : "");
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
        const photo = {
            id: this.photo != null ? this.photo.id : null,
            name: this._nameInputElement.value,
            imageUrl: this._imageUrlInputElement.value,
            orderIndex: this._orderIndexInputElement.value,
            description: this.descriptionEditor.text
        } as Photo;
        
        this.dispatchEvent(new PhotoAdd(photo));            
    }

    public onCreate() {        
        this.dispatchEvent(new PhotoEdit(new Photo()));            
    }

    public onDelete() {        
        const photo = {
            id: this.photo != null ? this.photo.id : null,
            name: this._nameInputElement.value
        } as Photo;

        this.dispatchEvent(new PhotoDelete(photo));         
    }

    attributeChangedCallback(name, oldValue, newValue) {
        switch (name) {
            case "photo-id":
                this.photoId = newValue;
                break;
            case "photo":
                this.photo = JSON.parse(newValue);
                if (this.parentNode) {
                    this.photoId = this.photo.id;
                    this._nameInputElement.value = this.photo.name != undefined ? this.photo.name : "";
                    this._orderIndexInputElement.value = this.photo.orderIndex != undefined ? this.photo.orderIndex : "";
                    this._imageUrlInputElement.value = this.photo.imageUrl != undefined ? this.photo.imageUrl : "";
                    this.descriptionEditor.setHTML(this.photo.description != undefined ? this.photo.description : "");
                    this._titleElement.textContent = this.photoId ? "Edit Photo" : "Create Photo";
                }
                break;
        }           
    }

    public photoId: any;
    
	public photo: Photo;

    public descriptionEditor: EditorComponent;

    private get _createButtonElement(): HTMLElement { return this.querySelector(".photo-create") as HTMLElement; }
    
	private get _titleElement(): HTMLElement { return this.querySelector("h2") as HTMLElement; }
    
	private get _saveButtonElement(): HTMLElement { return this.querySelector(".save-button") as HTMLElement };
    
	private get _deleteButtonElement(): HTMLElement { return this.querySelector(".delete-button") as HTMLElement };
    
    private get _nameInputElement(): HTMLInputElement { return this.querySelector(".photo-name") as HTMLInputElement; }

    private get _orderIndexInputElement(): HTMLInputElement { return this.querySelector(".photo-order-index") as HTMLInputElement; }

    private get _imageUrlInputElement(): HTMLInputElement { return this.querySelector(".photo-image-url") as HTMLInputElement; }

    private get _descriptionInputElement(): HTMLInputElement { return this.querySelector(".photo-description") as HTMLInputElement; }
}

customElements.define(`ce-photo-edit-embed`,PhotoEditEmbedComponent);
