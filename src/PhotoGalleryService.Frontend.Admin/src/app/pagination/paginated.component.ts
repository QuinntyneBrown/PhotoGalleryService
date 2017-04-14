import { toPageListFromInMemory } from "./";

export abstract class PaginatedComponent<T> extends HTMLElement {
    constructor(public pageSize: number, public pageNumber:number, private _nextCssClass:string, private _previousCssClass: string) {
        super();
        this.onNext = this.onNext.bind(this);
        this.onPrevious = this.onPrevious.bind(this);           
    }
    
    connectedCallback(options: { template: string, styles: string }) {        
        this.innerHTML = `<style>${options.styles}</style> ${options.template}`;        
        this._nextElement.addEventListener("click", this.onNext);
        this._previousElement.addEventListener("click", this.onPrevious);
        this.setEventListeners();
        this.bind();        
    }
    
    public setEventListeners() {

    }

    public abstract bind();

    disconnectedCallback() {
        this._nextElement.removeEventListener("click", this.onNext);
        this._previousElement.removeEventListener("click", this.onPrevious);
    }

    public abstract render();
    public pagedList: IPagedList<T>;
    public entities: Array<T>;

    public onNext(e: Event) {        
        e.stopPropagation();

        if (this.pageNumber == this.pagedList.totalPages) {
            this.pageNumber = 1;
        } else {
            this.pageNumber = this.pageNumber + 1;
        }
        this.render();
    }

    public onPrevious(e: Event) {
        e.stopPropagation();

        if (this.pageNumber == 1) {
            this.pageNumber = this.pagedList.totalPages;
        } else {
            this.pageNumber = this.pageNumber - 1;
        }
        this.render();
    }

    private get _nextElement(): HTMLElement { return this.querySelector(this._nextCssClass) as HTMLElement; }

    private get _previousElement(): HTMLElement { return this.querySelector(this._previousCssClass) as HTMLElement; }
}