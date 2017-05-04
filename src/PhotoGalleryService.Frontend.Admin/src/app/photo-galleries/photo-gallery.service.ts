import { fetch } from "../utilities";
import { PhotoGallery } from "./photo-gallery.model";
import { environment } from "../environment";

export class PhotoGalleryService {
    constructor(private _fetch = fetch) { }

    private static _instance: PhotoGalleryService;

    public static get Instance() {
        this._instance = this._instance || new PhotoGalleryService();
        return this._instance;
    }

    public get(): Promise<Array<PhotoGallery>> {
        return this._fetch({ url: `${environment.baseUrl}api/photogallery/get`, authRequired: true }).then((results:string) => {
            return (JSON.parse(results) as { photoGalleries: Array<PhotoGallery> }).photoGalleries;
        });
    }

    public getById(id): Promise<PhotoGallery> {
        return this._fetch({ url: `${environment.baseUrl}api/photogallery/getbyid?id=${id}`, authRequired: true }).then((results:string) => {
            return (JSON.parse(results) as { photoGallery: PhotoGallery }).photoGallery;
        });
    }

    public add(photoGallery) {
        return this._fetch({ url: `${environment.baseUrl}api/photogallery/add`, method: `POST`, data: { photoGallery }, authRequired: true  });
    }

    public remove(options: { id : number }) {
        return this._fetch({ url: `${environment.baseUrl}api/photogallery/remove?id=${options.id}`, method: `DELETE`, authRequired: true  });
    }
    
}
