import { fetch } from "../utilities";
import { Photo } from "./photo.model";
import { environment } from "../environment";

export class PhotoService {
    constructor(private _fetch = fetch) { }

    private static _instance: PhotoService;

    public static get Instance() {
        this._instance = this._instance || new PhotoService();
        return this._instance;
    }

    public get(): Promise<Array<Photo>> {
        return this._fetch({ url: `${environment.baseUrl}api/photo/get`, authRequired: true }).then((results:string) => {
            return (JSON.parse(results) as { photos: Array<Photo> }).photos;
        });
    }

    public getById(id): Promise<Photo> {
        return this._fetch({ url: `${environment.baseUrl}api/photo/getbyid?id=${id}`, authRequired: true }).then((results:string) => {
            return (JSON.parse(results) as { photo: Photo }).photo;
        });
    }

    public add(photo) {
        return this._fetch({ url: `${environment.baseUrl}api/photo/add`, method: `POST`, data: { photo }, authRequired: true  });
    }

    public remove(options: { id : number }) {
        return this._fetch({ url: `${environment.baseUrl}api/photo/remove?id=${options.id}`, method: `DELETE`, authRequired: true  });
    }
    
}
