import {Component, input, InputSignal} from '@angular/core';
import {NgOptimizedImage} from "@angular/common";
import {PhotoItem} from '../../models/AlbumInterfacing';
import {AdminViewPhotoCard} from '../admin-view-photo-card/admin-view-photo-card';

@Component({
  selector: 'app-photos-display',
    imports: [
        AdminViewPhotoCard
    ],
  templateUrl: './photos-display.html',
  styleUrl: './photos-display.css',
})
export class PhotosDisplay {
  public readonly displayPhotos = input<PhotoItem[]>([]);
}
