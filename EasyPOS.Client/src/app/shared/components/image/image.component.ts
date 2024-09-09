import { Component, EventEmitter, Input, Output } from '@angular/core';
import { SafeUrl } from '@angular/platform-browser';

@Component({
  selector: 'app-image',
  templateUrl: './image.component.html',
  styleUrl: './image.component.scss'
})
export class ImageComponent {
  @Input() hide: boolean = false;
  @Input() imageClass: string | null = null;
  @Input() imageStyle: { [klass: string]: any; } | null = null;
  @Input() styleClass: string | null = null;
  @Input() style: { [klass: string]: any; } | null = null;
  @Input() src: string | SafeUrl | null = null;
  @Input() srcSet: string | SafeUrl | null = null;
  @Input() sizes: string | null = null;
  @Input() previewImageSrc: string | SafeUrl | null = null;
  @Input() previewImageSrcSet: string | SafeUrl | null = null;
  @Input() previewImageSizes: string | null = null;
  @Input() alt: string | null = null;
  @Input() width: string | null = null;
  @Input() height: string | null = null;
  @Input() loading: 'eager' | 'lazy' | null = null;
  @Input() appendTo: any = null;
  @Input() preview: boolean = false;
  @Input() circle: boolean = false;
  @Input() border: boolean = false;
  @Input() showTransitionOptions: string = '150ms cubic-bezier(0, 0, 0.2, 1)';
  @Input() hideTransitionOptions: string = '150ms cubic-bezier(0, 0, 0.2, 1)';
  @Output() onShow = new EventEmitter<any>();
  @Output() onHide = new EventEmitter<any>();
  @Output() onImageError = new EventEmitter<any>();

  showImage(event: any){
    console.log(event)
    this.styleClass = undefined;
    this.onShow.emit(event);
  }
  hideImage(event: any){
    console.log(event)
    this.onHide.emit(event);
  }
  imageImage(event: any){
    this.onImageError.emit(event);
  }
}
