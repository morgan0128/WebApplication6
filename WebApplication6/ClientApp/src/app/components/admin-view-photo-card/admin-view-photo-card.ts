import {Component, computed, inject, input, signal} from '@angular/core';
import {PhotoItem} from '../../models/AlbumInterfacing';
import {NgOptimizedImage} from '@angular/common';
import { AlbumApiCaller } from '../../services/album-api-caller';
import {CdkDrag, CdkDragHandle} from '@angular/cdk/drag-drop';

@Component({
  selector: 'app-admin-view-photo-card',
  imports: [
    NgOptimizedImage,
    // CdkDrag,
    // CdkDragHandle
  ],
  templateUrl: './admin-view-photo-card.html',
  styleUrl: './admin-view-photo-card.css',
})
export class AdminViewPhotoCard {
  private readonly albumApi = inject(AlbumApiCaller);

  albumId = input.required<number>();
  photo = input.required<PhotoItem>();
  togglingDisplaysName = signal<boolean>(false);


  // testing
  // displaysName = computed<string | null>(() => this.photo().description);

  // actual? but is just redundant...
  displaysNameColor = computed<string | null>(() => (this.photo().displaysName) ? 'green' : 'red');
  // instead access variable from photo (when needed, in the template)?

  toggleDisplaysName(){
    if (this.photo().id == null) return;
    this.togglingDisplaysName.set(true);
    let request = this.albumApi.toggleDisplaysName(this.albumId(), this.photo().id!);
    request.subscribe({
      next: () => {
        this.photo().displaysName = !this.photo().displaysName;
        this.togglingDisplaysName.set(false);
        return;
      },
      error: () => {
        this.togglingDisplaysName.set(false);
        return;
      }
    })
  }

  // saveChanges(){
  // }
}
