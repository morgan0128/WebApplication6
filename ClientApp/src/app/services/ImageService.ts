// import { Injectable } from '@angular/core';
// import { HttpClient } from '@angular/common/http';
// import {forkJoin, Observable} from 'rxjs';
//
// @Injectable({
//   providedIn: 'root'
// })
//
// export class ImageService {
//   private apiUrl = '/api/Image';
//
//   constructor(private http: HttpClient) {}
//
//   getImages(): Observable<ImageDto[]> {
//     return this.http.get<ImageDto[]>('/api/Image');
//   }
//
//
//   // getFileStream(): Observable<Blob>[] {
//   //   return this.http.get(this.apiUrl, { responseType: 'blob' });
//   // }
//   //
//   // public downloadMultipleFiles(fileUrls: string[]): Observable<Blob[]> {
//   //   const requests = fileUrls.map(url =>
//   //     this.http.get(url, { responseType: 'blob' })
//   //   );
//   //   return forkJoin(requests);
//   // }
// }
//
// export interface ImageDto {
//   id: number;
//   fileName: string;
//   contentType: string;
//   fileSize: number;
//   storagePath: string;
//   altText: string | null;
// }
//
//
