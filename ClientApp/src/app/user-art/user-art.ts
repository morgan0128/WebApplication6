import {Component, effect, inject, OnInit, signal} from '@angular/core';
import {HttpClient} from '@angular/common/http';
// import {DatePipe} from '@angular/common';
import {FormsModule} from '@angular/forms';
// import {ImageService, ImageDto} from '../services/ImageService';
// import { ImageService } from '../services/ImageService'
import { ImageLoader } from '../image-loader/image-loader'

interface ImageDTO {
  id: number;
  fileName: string;
  contentType: string;
  fileSize: number;
  storagePath: string;
  altText: string | null;
}

@Component({
  selector: 'app-user-art',
  imports: [FormsModule, ImageLoader],
  templateUrl: './user-art.html',
  styleUrl: './user-art.css',
})
export class UserArt implements OnInit {
  private readonly http = inject(HttpClient);

  private readonly apiImageUrl = '/api/Image';

  private imageLoader = new ImageLoader();

  // constructor(private imageService: ImageService) {}
  // protected readonly images = signal<Image[]>([]);
  // protected readonly imagesFileInfo = signal<FileInfo[]>([]);
  // protected blobs = signal<Blob[]>([]);
  // protected urls = signal<string[]>([]);
  // images = signal<MediaSource[]>([]);
  // urls = signal<string[]>([]);
  urls: string[] = [];

  protected readonly imagesIsLoading = signal(true);
  protected readonly imageDTOLoadingError = signal<string | null>(null);
  protected readonly imagesLoadingError = signal<string | null>(null);

  selectedFile: File | null = null;
  previewImageUrl: string | null = null;
  imageUploadError: string | null = null;
  imageIsUploading = false;

  protected readonly imageError = signal<string | null>(null);

  protected readonly images = signal<ImageDTO[]>([]);

  // protected readonly imagesMedia = signal<MediaSource[]>([]);

  ngOnInit(): void {
    this.loadImageDTOs();
  }

  // constructor() {
  //   effect(() => {
  //     this.loadImages();
  //   })
  // }

  // protected loadImages(): void {
  //   this.imagesIsLoading.set(true);
  //   this.imagesLoadingError.set(null);
  //
  //   // this.
  //
  //   // return this.http.get<Blob[]>(this.apiImageUrl).subscribe
  // }

  //  loadImages(): void {
  //   this.imagesIsLoading.set(true);
  //   this.imageDTOLoadingError.set(null);
  //   this.imagesLoadingError.set(null);
  //
  //   this.loadImageDTOs();
  //
  //
  //   if (this.imageDTOLoadingError()){
  //     this.imagesIsLoading.set(false);
  //     this.imagesLoadingError.set('Could not load image DTOs');
  //     return;
  //   }
  //
  //
  //    this.images().forEach(image => {
  //      var path = this.apiImageUrl + "/" + (image.id);
  //      this.http.get(path, {responseType: 'blob'}).subscribe({
  //        next: blob => {
  //          this.urls.push(URL.createObjectURL(blob));
  //          console.log(this.urls);
  //        },
  //        error: () => {
  //          this.imagesIsLoading.set(false);
  //          this.imagesLoadingError.set('Could not complete loading images from DTOs');
  //          console.log(this.imagesLoadingError);
  //          this.imagesIsLoading.set(false);
  //        }
  //      });
  //    })
  //
  //    this.imagesIsLoading.set(false);
  //
  //
  // }

  loadImage(): void{
    this.imageLoader.loadImage();
  }

  private loadImageDTOs(): void {
    this.http.get<ImageDTO[]>(this.apiImageUrl).subscribe({
      next: dto => {
        this.images.set(dto);
        console.log("DTO Success: " + dto);
      },
      error: () => {
        this.imageDTOLoadingError.set('Could not load images');
        console.log(this.imageDTOLoadingError);
      }
    });
}

  protected onFileSelected(event: Event): void {
    this.imageUploadError = null;

    const input = event.target as HTMLInputElement;
    const file = input.files?.[0];

    if (!file){
      return;
    }

    const allowedTypes = ["image/jpeg", "image/png", "image/webp"];
    if (!allowedTypes.includes(file.type)) {
      this.imageUploadError = "Choos a JPG, PNG, or WebP image.";
      this.selectedFile = null;
      return;
    }

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

}
