import { Photo } from "./photo.model";

export const photoActions = {
    ADD: "[Photo] Add",
    EDIT: "[Photo] Edit",
    DELETE: "[Photo] Delete",
    PHOTOS_CHANGED: "[Photo] Photos Changed"
};

export class PhotoEvent extends CustomEvent {
    constructor(eventName:string, photo: Photo) {
        super(eventName, {
            bubbles: true,
            cancelable: true,
            detail: { photo }
        });
    }
}

export class PhotoAdd extends PhotoEvent {
    constructor(photo: Photo) {
        super(photoActions.ADD, photo);        
    }
}

export class PhotoEdit extends PhotoEvent {
    constructor(photo: Photo) {
        super(photoActions.EDIT, photo);
    }
}

export class PhotoDelete extends PhotoEvent {
    constructor(photo: Photo) {
        super(photoActions.DELETE, photo);
    }
}

export class PhotosChanged extends CustomEvent {
    constructor(photos: Array<Photo>) {
        super(photoActions.PHOTOS_CHANGED, {
            bubbles: true,
            cancelable: true,
            detail: { photos }
        });
    }
}
