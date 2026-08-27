import {Component, computed, input, signal} from '@angular/core';
import {PhotoItem} from '../../models/AlbumInterfacing';
import {NgOptimizedImage} from '@angular/common';

@Component({
  selector: 'app-admin-view-photo-card',
  imports: [
    NgOptimizedImage
  ],
  templateUrl: './admin-view-photo-card.html',
  styleUrl: './admin-view-photo-card.css',
})
export class AdminViewPhotoCard {
  photo = input.required<PhotoItem>();

  // testing
  // displaysName = computed<string | null>(() => this.photo().description);

  // actual? but is just redundant...
  // displaysName = computed<string | null>(() => this.photo().displaysName);
  // instead access variable from photo (when needed, in the template)?


  saveChanges(){

  }
}
