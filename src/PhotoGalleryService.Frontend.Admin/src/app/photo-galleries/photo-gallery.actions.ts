import { PhotoGallery } from "./photo-gallery.model";

export const photoGalleryActions = {
    ADD: "[PhotoGallery] Add",
    EDIT: "[PhotoGallery] Edit",
    DELETE: "[PhotoGallery] Delete",
    PHOTO_GALLERYS_CHANGED: "[PhotoGallery] PhotoGallerys Changed"
};

export class PhotoGalleryEvent extends CustomEvent {
    constructor(eventName:string, photoGallery: PhotoGallery) {
        super(eventName, {
            bubbles: true,
            cancelable: true,
            detail: { photoGallery }
        });
    }
}

export class PhotoGalleryAdd extends PhotoGalleryEvent {
    constructor(photoGallery: PhotoGallery) {
        super(photoGalleryActions.ADD, photoGallery);        
    }
}

export class PhotoGalleryEdit extends PhotoGalleryEvent {
    constructor(photoGallery: PhotoGallery) {
        super(photoGalleryActions.EDIT, photoGallery);
    }
}

export class PhotoGalleryDelete extends PhotoGalleryEvent {
    constructor(photoGallery: PhotoGallery) {
        super(photoGalleryActions.DELETE, photoGallery);
    }
}

export class PhotoGallerysChanged extends CustomEvent {
    constructor(photoGallerys: Array<PhotoGallery>) {
        super(photoGalleryActions.PHOTO_GALLERYS_CHANGED, {
            bubbles: true,
            cancelable: true,
            detail: { photoGallerys }
        });
    }
}
