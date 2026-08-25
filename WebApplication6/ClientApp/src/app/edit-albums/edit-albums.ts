import {Component, inject, OnInit, signal, Signal} from '@angular/core';
import {FormsModule} from '@angular/forms';
// import {NgOptimizedImage} from '@angular/common';
import {HttpClient} from '@angular/common/http';
import {NgOptimizedImage} from '@angular/common';

interface AlbumDTO{
  id: number,
  name: string | null,
  description: string | null,
}

class PhotoDTO {
  id: number | null = null;
  name: string | null = null;
  description: string | null = null;
  yearContentCreated: number | null = null;
  image: ImageDTO | null = null;
}

interface ImageDTO {
  id: number,
  fileName: string,
  contentType: string,
  fileSize: number | null,
  storageFileName: string,
  url: string,
  altText: string,
  width: number,
  height: number
}

@Component({
  selector: 'app-edit-albums',
  imports: [
    FormsModule,
    NgOptimizedImage
  ],
  templateUrl: './edit-albums.html',
  styleUrl: './edit-albums.css',
})
export class EditAlbums implements OnInit {
  private readonly http = inject(HttpClient);
  private readonly apiAlbumUrl = '/api/Album';

  protected readonly creatingAlbum = signal<boolean>(false);
  protected readonly creatingAlbumError = signal<boolean>(false);
  protected newAlbumName = '';
  protected newAlbumDescription = '';

  protected readonly loadingAlbums = signal<boolean>(true);
  protected readonly loadingAlbumsError = signal<boolean>(false);

  protected selectedAlbum: AlbumDTO | null = null;
  // protected readonly selectedAlbumId = signal<number | null>(null);
  selectedAlbumId: number | null = null;
  protected readonly selectingAlbumError = signal<boolean>(false);

  protected newPhotoName = '';
  protected newPhotoDescription = '';
  newPhotoSelectedImageFile: File | null = null;
  protected previewImageUrl: string | null = null;
  protected imagePreviewUploadError: string | null = null;
  protected imagePreviewUploading = false;

  protected readonly uploadingPhoto = signal<boolean>(false);
  protected uploadPhotoError: string | null = null;

  protected readonly loadingPhotos = signal<boolean>(true);
  protected readonly loadingPhotosError = signal<boolean>(false);

  protected readonly albumDTOs = signal<AlbumDTO[]>([]);
  protected readonly photoDTOs = signal<PhotoDTO[]>([]);

  ngOnInit() {
    this.loadAlbums();
  }

  loadAlbums(){
    this.loadingAlbums.set(true);

    let requestPath = this.apiAlbumUrl + '/all';
    this.http.get<AlbumDTO[]>(requestPath).subscribe({
      next: dtos => {
        this.albumDTOs.set(dtos);
        this.loadingAlbums.set(false);
      },
      error: () => {
        this.loadingAlbums.set(false);
        this.loadingAlbumsError.set(true);
      }
    })
  }

  createAlbum(){
    this.creatingAlbum.set(true);
    this.creatingAlbumError.set(false);
    const name = this.newAlbumName.trim();
    const description = this.newAlbumDescription.trim();

    let requestPath = this.apiAlbumUrl + '';
    this.http.post(requestPath, { name, description }).subscribe({
      next: () => {
        this.creatingAlbum.set(false);
        this.loadAlbums();
      },
      error: () => {
        this.creatingAlbumError.set(true);
        this.creatingAlbum.set(false);
      }
    })
  }

  onSelectedAlbumChange(){
    // return;

    if (this.selectedAlbumId == null){
      this.selectedAlbum = null;
      this.photoDTOs.set([]);
      return; // default selection value
    }
    if (this.selectedAlbum?.id == this.selectedAlbumId){
      return; // already selected
    }

    this.selectingAlbumError.set(false);

    const albumDTO = this.albumDTOs().find(a => a.id == this.selectedAlbumId);

    if (!albumDTO){
      this.selectingAlbumError.set(true);
      return;
    }

    this.selectedAlbum = albumDTO;
    this.photoDTOs.set([]);
    this.loadAlbumSelection().then(r => { return; });
  }

  async loadAlbumSelection(){
    this.loadingPhotos.set(true);
    this.loadingPhotosError.set(false);
    this.photoDTOs.set([]);

    let requestPath = this.apiAlbumUrl + '/' + this.selectedAlbumId + '/photos';
    this.http.get<PhotoDTO[]>(requestPath).subscribe({
      next: dtos => {
        this.photoDTOs.set(dtos);
      },
      error: () => {
        this.loadingPhotos.set(false);
        this.loadingPhotosError.set(true);
      },
      complete: () => {
        this.loadingPhotos.set(false);
      }
    })
  }

  protected onFileSelected(event: Event): void {
    this.imagePreviewUploadError = null;

    const input = event.target as HTMLInputElement;
    const file = input.files?.[0];

    if (!file) {
      return;
    }

    const maxSizeMb = 5;
    if (file.size > maxSizeMb * 1024 * 1024) {
      this.imagePreviewUploadError = `Image must be under ${maxSizeMb} MB.`
      this.newPhotoSelectedImageFile = null;
      return;
    }

    this.newPhotoSelectedImageFile = file;
    this.previewImageUrl = URL.createObjectURL(file);
  }

  uploadPhotoToSelected(){
    if (this.newPhotoSelectedImageFile == null){
      return;
    }

    this.uploadingPhoto.set(true);
    this.uploadPhotoError = null;
    const formData: FormData = new FormData();

    const name = this.newPhotoName.trim();
    const description = this.newPhotoDescription.trim();
    let yearContentCreated = 2003;

    const photoSpec = new PhotoDTO();
    photoSpec.name = name;
    photoSpec.description = description;
    photoSpec.yearContentCreated = yearContentCreated;

    formData.append('file', this.newPhotoSelectedImageFile, this.newPhotoSelectedImageFile.name);
    formData.append('name', name);
    formData.append('description', description)
    formData.append('yearContentCreated', yearContentCreated.toString());

    console.log(formData.getAll('file'));
    console.log(formData.getAll('photoSpec'));

    let requestPath = this.apiAlbumUrl + '/' + this.selectedAlbumId + '/upload';
    this.http.post(requestPath, formData).subscribe({
      next: () => {
        this.uploadingPhoto.set(false);
        this.loadAlbumSelection().then(r => { return; }); // TODO: currently performs an excessive full reload
        // this.loadAlbums();
      },
      error: () => {
        this.uploadPhotoError = "error";
        this.uploadingPhoto.set(false);
      }
    })
  }



}
