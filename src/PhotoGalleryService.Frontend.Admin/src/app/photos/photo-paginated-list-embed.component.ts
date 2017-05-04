import { Photo } from "./photo.model";
import { toPageListFromInMemory, PaginatedComponent } from "../pagination";

const template = require("./photo-paginated-list-embed.component.html");
const styles = require("./photo-paginated-list-embed.component.scss");

export class PhotoPaginatedListEmbedComponent extends PaginatedComponent<Photo> {
    constructor(
        private _document: Document = document
    ) {
        super(5, 1, ".next", ".previous");
    }


    static get observedAttributes() {
        return [
            "photos"
        ];
    }

    connectedCallback() {
        super.connectedCallback({ template, styles });  
        this.setEventListeners();
    }

    public bind() {        
    
    }

    public render() {
        this.pagedList = toPageListFromInMemory(this.entities, this.pageNumber, this.pageSize);
        this._totalPagesElement.textContent = JSON.stringify(this.pagedList.totalPages);
        this._currentPageElement.textContent = JSON.stringify(this.pageNumber);

        this._containerElement.innerHTML = "";
        for (let i = 0; i < this.pagedList.data.length; i++) {            
            const el = this._document.createElement(`ce-photo-item-embed`);
            el.setAttribute("entity", JSON.stringify(this.pagedList.data[i]));
            this._containerElement.appendChild(el);
        }
    }

    private get _currentPageElement(): HTMLElement { return this.querySelector(".current-page") as HTMLElement; }

    private get _totalPagesElement(): HTMLElement { return this.querySelector(".total-pages") as HTMLElement; }

    private get _containerElement(): HTMLElement { return this.querySelector(".container") as HTMLElement; }
    
    photos:Array<Photo> = [];

    attributeChangedCallback(name, oldValue, newValue) {
        switch (name) {
            case "photos":
                this.entities = JSON.parse(newValue);
                this.render();
                break;
        }
    }
}

customElements.define("ce-photo-paginated-list-embed", PhotoPaginatedListEmbedComponent);
