import { PhotoGallerySlide } from "./photo-gallery-slide.model";

export const photoGallerySlideActions = {
    ADD: "[PhotoGallerySlide] Add",
    EDIT: "[PhotoGallerySlide] Edit",
    DELETE: "[PhotoGallerySlide] Delete",
    PHOTO_GALLERY_SLIDES_CHANGED: "[PhotoGallerySlide] PhotoGallerySlides Changed"
};

export class PhotoGallerySlideEvent extends CustomEvent {
    constructor(eventName:string, photoGallerySlide: PhotoGallerySlide) {
        super(eventName, {
            bubbles: true,
            cancelable: true,
            detail: { photoGallerySlide }
        });
    }
}

export class PhotoGallerySlideAdd extends PhotoGallerySlideEvent {
    constructor(photoGallerySlide: PhotoGallerySlide) {
        super(photoGallerySlideActions.ADD, photoGallerySlide);        
    }
}

export class PhotoGallerySlideEdit extends PhotoGallerySlideEvent {
    constructor(photoGallerySlide: PhotoGallerySlide) {
        super(photoGallerySlideActions.EDIT, photoGallerySlide);
    }
}

export class PhotoGallerySlideDelete extends PhotoGallerySlideEvent {
    constructor(photoGallerySlide: PhotoGallerySlide) {
        super(photoGallerySlideActions.DELETE, photoGallerySlide);
    }
}

export class PhotoGallerySlidesChanged extends CustomEvent {
    constructor(photoGallerySlides: Array<PhotoGallerySlide>) {
        super(photoGallerySlideActions.PHOTO_GALLERY_SLIDES_CHANGED, {
            bubbles: true,
            cancelable: true,
            detail: { photoGallerySlides }
        });
    }
}
