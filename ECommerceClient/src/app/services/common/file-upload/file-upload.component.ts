import { Component } from '@angular/core';
import { NgxFileDropEntry } from 'ngx-file-drop';

@Component({
  selector: 'app-file-upload',
  templateUrl: './file-upload.component.html',
  styleUrl: './file-upload.component.scss'
})
export class FileUploadComponent {
  public files: NgxFileDropEntry[];

  public selectedFiles(files: NgxFileDropEntry[]) {
    this.files = files;
    const fileData : FormData = new FormData();
    for (const file of files) {
      (file.fileEntry as FileSystemFileEntry).file((_file:File)=>{
        fileData.append(_file.name,_file,file.relativePath)
      })
    }

    
  }


}
