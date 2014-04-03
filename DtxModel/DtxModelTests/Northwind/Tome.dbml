<?xml version="1.0" encoding="utf-8"?><Database ContextNamespace="DtxModelTests.Tome" Class="TomeContext" xmlns="http://schemas.microsoft.com/linqtosql/dbml/2007">
  <Table Name="" Member="Volumes">
    <Type Name="Volumes">
      <Column Name="" Member="id" Storage="_rowid" AutoSync="Never" Type="System.Int32" IsReadOnly="true" IsPrimaryKey="true" IsDbGenerated="true" CanBeNull="false" />
      <Column Name="" Member="number" Storage="_Number" Type="System.Int32" CanBeNull="false" />
      <Column Name="" Member="description" Storage="_Description" Type="System.String" CanBeNull="false" IsDelayLoaded="true" />
      <Column Name="" Member="Manga_id" Storage="_MangaRowid" Type="System.Int32" CanBeNull="false" />
      <Association Name="Volumes_Pages" Member="Pages" ThisKey="id" OtherKey="Volumes_id" Type="Pages" />
      <Association Name="Manga_Volumes" Member="Manga" Storage="_MangaModel" ThisKey="Manga_id" OtherKey="id" Type="Manga" IsForeignKey="true" />
    </Type>
  </Table>
  <Table Name="" Member="Manga">
    <Type Name="Manga">
      <Column Name="" Member="id" Storage="_rowid" AutoSync="Never" Type="System.Int32" IsReadOnly="true" IsPrimaryKey="true" IsDbGenerated="true" CanBeNull="false" />
      <Column Name="" Member="status_enum" Storage="_Status" Type="System.Int32" CanBeNull="false" />
      <Column Name="" Member="last_updated" Storage="_LastUpdated" Type="System.DateTime" CanBeNull="false" />
      <Column Name="" Member="total_volumes_released" Storage="_TotalVolumesReleased" Type="System.Int32" CanBeNull="false" />
      <Column Name="" Member="manga_updates_id" Storage="_MangaUpdatesId" Type="System.Int32" CanBeNull="false" />
      <Column Name="" Member="year_released" Storage="_YearReleased" Type="System.Int32" CanBeNull="false" />
      <Column Name="" Member="description" Storage="_Description" Type="System.String" CanBeNull="false" />
      <Association Name="Manga_Volumes" Member="VolumesModels" ThisKey="id" OtherKey="Manga_id" Type="Volumes" />
      <Association Name="Manga_MangaGenres" Member="MangaGenres" ThisKey="id" OtherKey="Manga_id" Type="MangaGenres" />
      <Association Name="Manga_MangaCategories" Member="MangaCategories" ThisKey="id" OtherKey="Manga_id" Type="MangaCategories" />
      <Association Name="Manga_MangaPeople" Member="MangaPeople" ThisKey="id" OtherKey="Manga_id" Type="MangaPeople" />
      <Association Name="Manga_MangaTitles" Member="MangaTitles" ThisKey="id" OtherKey="Manga_id" Type="MangaTitles" Cardinality="One" />
    </Type>
  </Table>
  <Table Name="" Member="Tags">
    <Type Name="Tags">
      <Column Name="" Member="id" Storage="_rowid" AutoSync="Never" Type="System.Int32" IsReadOnly="true" IsPrimaryKey="true" IsDbGenerated="true" CanBeNull="false" />
      <Column Name="" Member="value" Storage="_Tag" Type="System.String" CanBeNull="false" />
      <Association Name="PageTags_Tags" Member="PageTags" ThisKey="id" OtherKey="Tags_id" Type="PageTags" IsForeignKey="true" />
    </Type>
  </Table>
  <Table Name="" Member="PageTags">
    <Type Name="PageTags">
      <Column Name="" Member="id" Storage="_rowid" AutoSync="Never" Type="System.Int32" IsReadOnly="true" IsPrimaryKey="true" IsDbGenerated="true" CanBeNull="false" />
      <Column Name="" Member="Pages_id" Storage="_PagesRowid" Type="System.Int32" CanBeNull="false" />
      <Column Name="" Member="Tags_id" Storage="_TagsRowid" Type="System.Int32" CanBeNull="false" />
      <Association Name="PageTags_Tags" Member="Tags" ThisKey="Tags_id" OtherKey="id" Type="Tags" Cardinality="One" />
      <Association Name="Pages_PageTags" Member="Pages" ThisKey="Pages_id" OtherKey="id" Type="Pages" IsForeignKey="true" />
    </Type>
  </Table>
  <Table Name="" Member="Pages">
    <Type Name="Pages">
      <Column Name="" Member="id" Storage="_rowid" AutoSync="Never" Type="System.Int32" IsReadOnly="true" IsPrimaryKey="true" IsDbGenerated="true" CanBeNull="false" />
      <Column Name="" Member="Files_id" Storage="_FilesRowid" Type="System.Int32" CanBeNull="false" />
      <Column Name="" Member="Volumes_id" Storage="_VolumesRowid" Type="System.Int32" CanBeNull="false" />
      <Association Name="Pages_PageTags" Member="PageTags" ThisKey="id" OtherKey="Pages_id" Type="PageTags" />
      <Association Name="Volumes_Pages" Member="Volume" Storage="_Volumes" ThisKey="Volumes_id" OtherKey="id" Type="Volumes" IsForeignKey="true" />
    </Type>
  </Table>
  <Table Name="" Member="Genres">
    <Type Name="Genres">
      <Column Name="" Member="id" Storage="_rowid" AutoSync="Never" Type="System.Int32" IsReadOnly="true" IsPrimaryKey="true" IsDbGenerated="true" CanBeNull="false" />
      <Column Name="" Member="value" Storage="_Value" Type="System.String" CanBeNull="false" />
      <Association Name="MangaGenres_Genres" Member="MangaGenres" ThisKey="id" OtherKey="Genres_id" Type="MangaGenres" IsForeignKey="true" />
    </Type>
  </Table>
  <Table Name="" Member="MangaGenres">
    <Type Name="MangaGenres">
      <Column Name="" Member="Id" Storage="_rowid" AutoSync="Never" Type="System.Int32" IsReadOnly="true" IsPrimaryKey="true" IsDbGenerated="true" CanBeNull="false" />
      <Column Name="" Member="Manga_id" Storage="_PagesRowid" Type="System.Int32" CanBeNull="false" />
      <Column Name="" Member="Genres_id" Storage="_TagsRowid" Type="System.Int32" CanBeNull="false" />
      <Association Name="MangaGenres_Genres" Member="Genres" ThisKey="Genres_id" OtherKey="id" Type="Genres" />
      <Association Name="Manga_MangaGenres" Member="Manga" ThisKey="Manga_id" OtherKey="id" Type="Manga" IsForeignKey="true" />
    </Type>
  </Table>
  <Table Name="" Member="Categories">
    <Type Name="Categories">
      <Column Name="" Member="id" Storage="_rowid" AutoSync="Never" Type="System.Int32" IsReadOnly="true" IsPrimaryKey="true" IsDbGenerated="true" CanBeNull="false" />
      <Column Name="" Member="value" Storage="_Value" Type="System.String" CanBeNull="false" />
      <Association Name="MangaCategories_Categories" Member="MangaCategories" ThisKey="id" OtherKey="Categories_id" Type="MangaCategories" IsForeignKey="true" />
    </Type>
  </Table>
  <Table Name="" Member="MangaCategories">
    <Type Name="MangaCategories">
      <Column Name="" Member="id" Storage="_rowid" AutoSync="Never" Type="System.Int32" IsReadOnly="true" IsPrimaryKey="true" IsDbGenerated="true" CanBeNull="false" />
      <Column Name="" Member="Manga_id" Storage="_PagesRowid" Type="System.Int32" CanBeNull="false" />
      <Column Name="" Member="Categories_id" Storage="_TagsRowid" Type="System.Int32" CanBeNull="false" />
      <Association Name="MangaCategories_Categories" Member="Categories" ThisKey="Categories_id" OtherKey="id" Type="Categories" />
      <Association Name="Manga_MangaCategories" Member="Manga" ThisKey="Manga_id" OtherKey="id" Type="Manga" IsForeignKey="true" />
    </Type>
  </Table>
  <Table Name="" Member="People">
    <Type Name="People">
      <Column Name="" Member="id" Storage="_rowid" AutoSync="Never" Type="System.Int32" IsReadOnly="true" IsPrimaryKey="true" IsDbGenerated="true" CanBeNull="false" />
      <Column Name="" Member="name" Storage="_Value" Type="System.String" CanBeNull="false" />
      <Association Name="MangaPeople_People" Member="MangaPeople" ThisKey="id" OtherKey="People_id" Type="MangaPeople" IsForeignKey="true" />
    </Type>
  </Table>
  <Table Name="" Member="MangaPeople">
    <Type Name="MangaPeople">
      <Column Name="" Member="id" Storage="_rowid" AutoSync="Never" Type="System.Int32" IsReadOnly="true" IsPrimaryKey="true" IsDbGenerated="true" CanBeNull="false" />
      <Column Name="" Member="People_id" Storage="_PagesRowid" Type="System.Int32" CanBeNull="false" />
      <Column Name="" Member="Manga_id" Storage="_TagsRowid" Type="System.Int32" CanBeNull="false" />
      <Column Name="" Member="is_artist" Storage="_IsArtist" Type="System.Boolean" CanBeNull="false" />
      <Column Name="" Member="is_author" Storage="_IsAuthor" Type="System.Boolean" CanBeNull="false" />
      <Association Name="MangaPeople_People" Member="People" ThisKey="People_id" OtherKey="id" Type="People" />
      <Association Name="Manga_MangaPeople" Member="Manga" ThisKey="Manga_id" OtherKey="id" Type="Manga" IsForeignKey="true" />
    </Type>
  </Table>
  <Table Name="" Member="MangaTitles">
    <Type Name="MangaTitles">
      <Column Name="" Member="id" Storage="_rowid" AutoSync="Never" Type="System.Int32" IsReadOnly="true" IsPrimaryKey="true" IsDbGenerated="true" CanBeNull="false" />
      <Column Name="" Member="Manga_id" Storage="_TagsRowid" Type="System.Int32" CanBeNull="false" />
      <Column Name="" Member="value" Storage="_Value" Type="System.String" CanBeNull="false" />
      <Column Name="" Member="is_primary" Storage="_IsPrimary" Type="System.Boolean" CanBeNull="false" />
      <Association Name="Manga_MangaTitles" Member="Manga" ThisKey="Manga_id" OtherKey="id" Type="Manga" IsForeignKey="true" />
    </Type>
  </Table>
</Database>