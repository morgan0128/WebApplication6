import {Component, input} from '@angular/core';
import {AlbumItem, PhotoItem} from '../../models/AlbumInterfacing';
import {PhotosDisplay} from '../photos-display/photos-display';

@Component({
  selector: 'app-album-contents',
  imports: [PhotosDisplay],
  templateUrl: './album-contents.html',
  styleUrl: './album-contents.css',
})
export class AlbumContents {
  public readonly selectedAlbum = input<AlbumItem | null>(null);
  public readonly selectedAlbumId = input<number | null>(null);
  public readonly loadingPhotos = input<boolean>(false);
  public readonly photos = input<PhotoItem[]>([]);

}
