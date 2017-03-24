import { PhotoGallery } from "./photo-gallery.model";
import { PhotoGalleryService } from "./photo-gallery.service";
import { EditorComponent } from "../shared";
import { Router } from "../router";

const template = require("./photo-gallery-edit.component.html");
const styles = require("./photo-gallery-edit.component.scss");

export class PhotoGalleryEditComponent extends HTMLElement {
    constructor(
        private _photoGalleryService: PhotoGalleryService = PhotoGalleryService.Instance,
        private _router: Router = Router.Instance
        ) {
        super();
        this.onSave = this.onSave.bind(this);
        this.onDelete = this.onDelete.bind(this);
        this.onTitleClick = this.onTitleClick.bind(this);
    }

    static get observedAttributes() {
        return ["photo-gallery-id"];
    }
    
    connectedCallback() {        
        this.innerHTML = `<style>${styles}</style> ${template}`; 
        this._bind();
        this._setEventListeners();
    }
    
    private async _bind() {
        this._titleElement.textContent = this.photoGalleryId ? "Edit PhotoGallery": "Create PhotoGallery";

        if (this.photoGalleryId) {
            const photoGallery: PhotoGallery = await this._photoGalleryService.getById(this.photoGalleryId);                
            this._nameInputElement.value = photoGallery.name;  
        } else {
            this._deleteButtonElement.style.display = "none";
        }     
    }

    private _setEventListeners() {
        this._saveButtonElement.addEventListener("click", this.onSave);
        this._deleteButtonElement.addEventListener("click", this.onDelete);
        this._titleElement.addEventListener("click", this.onTitleClick);
    }

    private disconnectedCallback() {
        this._saveButtonElement.removeEventListener("click", this.onSave);
        this._deleteButtonElement.removeEventListener("click", this.onDelete);
        this._titleElement.removeEventListener("click", this.onTitleClick);
    }

    public async onSave() {
        const photoGallery = {
            id: this.photoGalleryId,
            name: this._nameInputElement.value
        } as PhotoGallery;
        
        await this._photoGalleryService.add(photoGallery);
        this._router.navigate(["photo-gallery","list"]);
    }

    public async onDelete() {        
        await this._photoGalleryService.remove({ id: this.photoGalleryId });
        this._router.navigate(["photo-gallery","list"]);
    }

    public onTitleClick() {
        this._router.navigate(["photo-gallery", "list"]);
    }

    attributeChangedCallback(name, oldValue, newValue) {
        switch (name) {
            case "photo-gallery-id":
                this.photoGalleryId = newValue;
                break;
        }        
    }

    public photoGalleryId: number;
    
    private get _titleElement(): HTMLElement { return this.querySelector("h2") as HTMLElement; }
    private get _saveButtonElement(): HTMLElement { return this.querySelector(".save-button") as HTMLElement };
    private get _deleteButtonElement(): HTMLElement { return this.querySelector(".delete-button") as HTMLElement };
    private get _nameInputElement(): HTMLInputElement { return this.querySelector(".photo-gallery-name") as HTMLInputElement;}
}

customElements.define(`ce-photo-gallery-edit`,PhotoGalleryEditComponent);
