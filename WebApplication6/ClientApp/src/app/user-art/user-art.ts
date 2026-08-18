import {Component, effect, inject, OnInit, signal} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {FormsModule} from '@angular/forms';
import { ImageLoader } from '../image-loader/image-loader'
import { NgOptimizedImage } from '@angular/common';

interface ImageDTO {
  id: number;
  fileName: string;
  contentType: string;
  fileSize: number;
  storageFileName: string;
  width: number;
  height: number;
  altText: string | null;
}

@Component({
  selector: 'app-user-art',
  imports: [FormsModule, NgOptimizedImage],
  templateUrl: './user-art.html',
  styleUrl: './user-art.css',
})
export class UserArt {
  private readonly http = inject(HttpClient);
  private readonly apiImageUrl = '/api/Image';

  // urls: string[] = [];

  selectedFile: File | null = null;
  previewImageUrl: string | null = null;
  imageUploadError: string | null = null;
  imageIsUploading = false;

  protected readonly loadedImages = signal<ImageDTO[]>([]);
  protected readonly imagesAreLoading = signal<boolean>(false);
  protected readonly imagesLoadError = signal<boolean>(false);

  protected loadAllImages(): void {
    this.imagesAreLoading.set(true);
    this.imagesLoadError.set(false);

    let requestPath = this.apiImageUrl + '/all';
    this.http.get<ImageDTO[]>(requestPath).subscribe({
      next: dtos => {
          this.loadedImages.set(dtos);
          this.imagesAreLoading.set(false);
      },
      error: () => {
        this.imagesAreLoading.set(false);
        this.imagesLoadError.set(true);
      }
    })
  }

  protected appendToLoadedImages(image: ImageDTO): void {
    if (this.inImages(image)){
      this.imagesAreLoading.set(true);
      this.loadedImages().push(image);
      this.imagesAreLoading.set(false);
    } else {
      return;
    }
  }

  private inImages(image: ImageDTO): boolean {
    const imageDTOs = this.loadedImages();

    if (imageDTOs.length == 0) return false;

    let found = false;
    let id = image.id;
    imageDTOs.forEach(i => {
      if (id == i.id){
        found = true;
      }
    });
      return found;
  }

  protected onFileSelected(event: Event): void {
    this.imageUploadError = null;

    const input = event.target as HTMLInputElement;
    const file = input.files?.[0];

    if (!file){
      return;
    }

    /* Maybe TODO */
    /* this approach for allowedTypes filter does not seem to work, but perhaps is unnecessary */

    // const allowedTypes = ["image/jpeg", "image/png", "image/webp"];
    // if (!allowedTypes.includes(file.type)) {
    //   this.imageUploadError = "Choos a JPG, PNG, or WebP image.";
    //   this.selectedFile = null;
    //   return;
    // }

    const maxSizeMb = 5;
    if (file.size > maxSizeMb * 1024 * 1024) {
      this.imageUploadError = 'Image must be under ${maxSizeMb} MB.}'
      this.selectedFile = null;
      return;
    }

    this.selectedFile = file;
    this.previewImageUrl = URL.createObjectURL(file);

  }

  protected uploadImage(): void {
    if (!this.selectedFile) {
      return;
    }

    const formData: FormData = new FormData();
    formData.append('file', this.selectedFile);

    this.imageIsUploading = true;

    this.http.post(this.apiImageUrl, formData).subscribe({
      next: (image) => {
        console.log('Uploaded:', image)
        this.imageIsUploading = false;
      },
      error: () => {
        this.imageUploadError = 'Upload failed.';
        this.imageIsUploading = false;
      }
    });
  }

  // protected deleteImage(image: Image): void {
  //   this.http.delete(`${this.apiImageUrl}/${image.id}`).subscribe({
  //     next: () => {
  //       this.images.update((images) => images.filter((item) => item.id !== image.id));
  //     },
  //     error: () => this.imageError.set('The item could not be deleted.')
  //   });
  // }

  protected readonly ImageLoader = ImageLoader;
}
