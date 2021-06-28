export interface ValueModel {
    Message: string;
  }
  
  export interface ContentModel {
    value: ValueModel;
  }
  
  export interface ExportExcelResponeModel {
    content: ContentModel;
    statusCode: number;
    reasonPhrase: string;
    isSuccessStatusCode: true;
  }
  