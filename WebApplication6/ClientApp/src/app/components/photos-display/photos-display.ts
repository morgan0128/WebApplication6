import {Component, input, InputSignal} from '@angular/core';
import {NgOptimizedImage} from "@angular/common";
import {PhotoItem} from '../../models/AlbumInterfacing';

@Component({
  selector: 'app-photos-display',
    imports: [
        NgOptimizedImage
    ],
  templateUrl: './photos-display.html',
  styleUrl: './photos-display.css',
})
export class PhotosDisplay {
  public readonly displayPhotos = input<PhotoItem[]>([]);
}
