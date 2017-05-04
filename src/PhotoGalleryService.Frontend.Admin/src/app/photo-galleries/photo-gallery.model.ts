import { Photo } from "../photos";

export class PhotoGallery { 

    public id:any;
    
    public name: string;

    public photos: Array<Photo> = [];

    public static fromJSON(data: any): PhotoGallery {

        let photoGallery = new PhotoGallery();

        photoGallery.name = data.name;

        photoGallery.photos = data.photos;

        return photoGallery;
    }
}
