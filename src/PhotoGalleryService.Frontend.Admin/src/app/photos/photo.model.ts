export class Photo { 

    public id:any;
    
    public name: string;

    public orderIndex: any;

    public imageUrl: string;

    public description: string;

    public static fromJSON(data: any): Photo {

        let photo = new Photo();

        photo.name = data.name;

        photo.orderIndex = data.orderIndex;

        photo.imageUrl = data.imageUrl;

        photo.description = data.description;

        return photo;
    }
}
