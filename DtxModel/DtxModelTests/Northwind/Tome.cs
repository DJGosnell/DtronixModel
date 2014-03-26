using System;
using System.Data.Common;
using System.Collections.Generic;
using DtxModel;

namespace DtxModelTests.Tome {

	public class TomeContext : Context {
		private static Func<DbConnection> _default_connection = null;

		/// <summary>
		/// Set a default constructor to allow use of parameterless context calling.
		/// </summary>
		public static Func<DbConnection> DefaultConnection {
			get { return _default_connection; }
			set { _default_connection = value; }
		}

		private Table<Volumes> _Volumes;

		public Table<Volumes> Volumes {
			get {
				if(_Volumes == null) {
					_Volumes = new Table<Volumes>(this);
				}

				return _Volumes;
			}
		}

		private Table<Manga> _Manga;

		public Table<Manga> Manga {
			get {
				if(_Manga == null) {
					_Manga = new Table<Manga>(this);
				}

				return _Manga;
			}
		}

		private Table<Tags> _Tags;

		public Table<Tags> Tags {
			get {
				if(_Tags == null) {
					_Tags = new Table<Tags>(this);
				}

				return _Tags;
			}
		}

		private Table<PageTags> _PageTags;

		public Table<PageTags> PageTags {
			get {
				if(_PageTags == null) {
					_PageTags = new Table<PageTags>(this);
				}

				return _PageTags;
			}
		}

		private Table<Pages> _Pages;

		public Table<Pages> Pages {
			get {
				if(_Pages == null) {
					_Pages = new Table<Pages>(this);
				}

				return _Pages;
			}
		}

		private Table<Genres> _Genres;

		public Table<Genres> Genres {
			get {
				if(_Genres == null) {
					_Genres = new Table<Genres>(this);
				}

				return _Genres;
			}
		}

		private Table<MangaGenres> _MangaGenres;

		public Table<MangaGenres> MangaGenres {
			get {
				if(_MangaGenres == null) {
					_MangaGenres = new Table<MangaGenres>(this);
				}

				return _MangaGenres;
			}
		}

		private Table<Categories> _Categories;

		public Table<Categories> Categories {
			get {
				if(_Categories == null) {
					_Categories = new Table<Categories>(this);
				}

				return _Categories;
			}
		}

		private Table<MangaCategories> _MangaCategories;

		public Table<MangaCategories> MangaCategories {
			get {
				if(_MangaCategories == null) {
					_MangaCategories = new Table<MangaCategories>(this);
				}

				return _MangaCategories;
			}
		}

		private Table<People> _People;

		public Table<People> People {
			get {
				if(_People == null) {
					_People = new Table<People>(this);
				}

				return _People;
			}
		}

		private Table<MangaPeople> _MangaPeople;

		public Table<MangaPeople> MangaPeople {
			get {
				if(_MangaPeople == null) {
					_MangaPeople = new Table<MangaPeople>(this);
				}

				return _MangaPeople;
			}
		}

		private Table<MangaTitles> _MangaTitles;

		public Table<MangaTitles> MangaTitles {
			get {
				if(_MangaTitles == null) {
					_MangaTitles = new Table<MangaTitles>(this);
				}

				return _MangaTitles;
			}
		}

		/// <summary>
		/// Create a new context of this database's type.  Can only be used if a default connection is specified.
		/// </summary>
		public TomeContext() : base(_default_connection) { }

		/// <summary>
		/// Create a new context of this database's type with a specific connection.
		/// </summary>
		/// <param name="connection">Existing open database connection to use.</param>
		public TomeContext(DbConnection connection) : base(connection) { }
	}

	[TableAttribute(Name = "Volumes")]
	public class Volumes : Model {
		private System.Int32 _rowid;
		public System.Int32 rowid {
			get { return _rowid; }
		}

		private bool _numberChanged = false;
		private System.Int32 _number;
		public System.Int32 number {
			get { return _number; }
			set {
				_number = value;
				_numberChanged = true;
			}
		}

		private bool _descriptionChanged = false;
		private System.String _description;
		public System.String description {
			get { return _description; }
			set {
				_description = value;
				_descriptionChanged = true;
			}
		}

		private bool _Manga_rowidChanged = false;
		private System.Int32 _Manga_rowid;
		public System.Int32 Manga_rowid {
			get { return _Manga_rowid; }
			set {
				_Manga_rowid = value;
				_Manga_rowidChanged = true;
			}
		}

		private Pages _Pages;
		public Pages Pages {
			get {
				if(_Pages == null){ 
					try {
						_Pages = ((TomeContext)context).Pages.select().whereIn("Volumes_rowid", _rowid).executeFetch();
					} catch {
						//Accessing a property outside of its database context is not allowed.  Access an association inside the database context to cache the values for later use.
						_Pages = null;
					}
				}
				return _Pages;
			}
		}

		private Manga[] _Manga;
		public Manga[] Manga {
			get {
				if(_Manga == null){ 
					try {
						_Manga = ((TomeContext)context).Manga.select().whereIn("rowid", _Manga_rowid).executeFetchAll();
					} catch {
						//Accessing a property outside of its database context is not allowed.  Access an association inside the database context to cache the values for later use.
						_Manga = null;
					}
				}
				return _Manga;
			}
		}

		public Volumes() : this(null, null) { }

		public Volumes(DbDataReader reader, Context context) {
			read(reader, context);
		}

		public override void read(DbDataReader reader, Context context) {
			this.context = context;
			if (reader == null) { return; }

			int length = reader.FieldCount;
			for (int i = 0; i < length; i++) {
				switch (reader.GetName(i)) {
					case "rowid": _rowid = (reader.IsDBNull(i)) ? default(System.Int32) : reader.GetInt32(i); break;
					case "number": _number = (reader.IsDBNull(i)) ? default(System.Int32) : reader.GetInt32(i); break;
					case "description": _description = reader.GetValue(i) as System.String; break;
					case "Manga_rowid": _Manga_rowid = (reader.IsDBNull(i)) ? default(System.Int32) : reader.GetInt32(i); break;
					default: break;
				}
			}
		}

		public override Dictionary<string, object> getChangedValues() {
			var changed = new Dictionary<string, object>();
			if (_numberChanged)
				changed.Add("number", _number);
			if (_descriptionChanged)
				changed.Add("description", _description);
			if (_Manga_rowidChanged)
				changed.Add("Manga_rowid", _Manga_rowid);

			return changed;
		}

		public override object[] getAllValues() {
			return new object[] {
				_number,
				_description,
				_Manga_rowid,
			};
		}

		public override string[] getColumns() {
			return new string[] {
				"number",
				"description",
				"Manga_rowid",
			};
		}

		public override string getPKName() {
			return "rowid";
		}

		public override object getPKValue() {
			return _rowid;
		}

	}

	[TableAttribute(Name = "Manga")]
	public class Manga : Model {
		private System.Int32 _rowid;
		public System.Int32 rowid {
			get { return _rowid; }
		}

		private bool _StatusEnumChanged = false;
		private System.Int32 _StatusEnum;
		public System.Int32 StatusEnum {
			get { return _StatusEnum; }
			set {
				_StatusEnum = value;
				_StatusEnumChanged = true;
			}
		}

		private bool _LastUpdatedChanged = false;
		private System.DateTime _LastUpdated;
		public System.DateTime LastUpdated {
			get { return _LastUpdated; }
			set {
				_LastUpdated = value;
				_LastUpdatedChanged = true;
			}
		}

		private bool _TotalVolumesReleasedChanged = false;
		private System.Int32 _TotalVolumesReleased;
		public System.Int32 TotalVolumesReleased {
			get { return _TotalVolumesReleased; }
			set {
				_TotalVolumesReleased = value;
				_TotalVolumesReleasedChanged = true;
			}
		}

		private bool _MangaUpdatesIdChanged = false;
		private System.Int32 _MangaUpdatesId;
		public System.Int32 MangaUpdatesId {
			get { return _MangaUpdatesId; }
			set {
				_MangaUpdatesId = value;
				_MangaUpdatesIdChanged = true;
			}
		}

		private bool _YearReleasedChanged = false;
		private System.Int32 _YearReleased;
		public System.Int32 YearReleased {
			get { return _YearReleased; }
			set {
				_YearReleased = value;
				_YearReleasedChanged = true;
			}
		}

		private bool _DescriptionChanged = false;
		private System.String _Description;
		public System.String Description {
			get { return _Description; }
			set {
				_Description = value;
				_DescriptionChanged = true;
			}
		}

		private Volumes _VolumesModels;
		public Volumes VolumesModels {
			get {
				if(_VolumesModels == null){ 
					try {
						_VolumesModels = ((TomeContext)context).Volumes.select().whereIn("Manga_rowid", _rowid).executeFetch();
					} catch {
						//Accessing a property outside of its database context is not allowed.  Access an association inside the database context to cache the values for later use.
						_VolumesModels = null;
					}
				}
				return _VolumesModels;
			}
		}

		private MangaGenres _MangaGenres;
		public MangaGenres MangaGenres {
			get {
				if(_MangaGenres == null){ 
					try {
						_MangaGenres = ((TomeContext)context).MangaGenres.select().whereIn("Manga_rowid", _rowid).executeFetch();
					} catch {
						//Accessing a property outside of its database context is not allowed.  Access an association inside the database context to cache the values for later use.
						_MangaGenres = null;
					}
				}
				return _MangaGenres;
			}
		}

		private MangaCategories _MangaCategories;
		public MangaCategories MangaCategories {
			get {
				if(_MangaCategories == null){ 
					try {
						_MangaCategories = ((TomeContext)context).MangaCategories.select().whereIn("Manga_rowid", _rowid).executeFetch();
					} catch {
						//Accessing a property outside of its database context is not allowed.  Access an association inside the database context to cache the values for later use.
						_MangaCategories = null;
					}
				}
				return _MangaCategories;
			}
		}

		private MangaPeople _MangaPeople;
		public MangaPeople MangaPeople {
			get {
				if(_MangaPeople == null){ 
					try {
						_MangaPeople = ((TomeContext)context).MangaPeople.select().whereIn("Manga_rowid", _rowid).executeFetch();
					} catch {
						//Accessing a property outside of its database context is not allowed.  Access an association inside the database context to cache the values for later use.
						_MangaPeople = null;
					}
				}
				return _MangaPeople;
			}
		}

		private MangaTitles _MangaTitles;
		public MangaTitles MangaTitles {
			get {
				if(_MangaTitles == null){ 
					try {
						_MangaTitles = ((TomeContext)context).MangaTitles.select().whereIn("Manga_rowid", _rowid).executeFetch();
					} catch {
						//Accessing a property outside of its database context is not allowed.  Access an association inside the database context to cache the values for later use.
						_MangaTitles = null;
					}
				}
				return _MangaTitles;
			}
		}

		public Manga() : this(null, null) { }

		public Manga(DbDataReader reader, Context context) {
			read(reader, context);
		}

		public override void read(DbDataReader reader, Context context) {
			this.context = context;
			if (reader == null) { return; }

			int length = reader.FieldCount;
			for (int i = 0; i < length; i++) {
				switch (reader.GetName(i)) {
					case "rowid": _rowid = (reader.IsDBNull(i)) ? default(System.Int32) : reader.GetInt32(i); break;
					case "status_enum": _StatusEnum = (reader.IsDBNull(i)) ? default(System.Int32) : reader.GetInt32(i); break;
					case "last_updated": _LastUpdated = (reader.IsDBNull(i)) ? default(System.DateTime) : reader.GetDateTime(i); break;
					case "total_volumes_released": _TotalVolumesReleased = (reader.IsDBNull(i)) ? default(System.Int32) : reader.GetInt32(i); break;
					case "manga_updates_id": _MangaUpdatesId = (reader.IsDBNull(i)) ? default(System.Int32) : reader.GetInt32(i); break;
					case "year_released": _YearReleased = (reader.IsDBNull(i)) ? default(System.Int32) : reader.GetInt32(i); break;
					case "description": _Description = reader.GetValue(i) as System.String; break;
					default: break;
				}
			}
		}

		public override Dictionary<string, object> getChangedValues() {
			var changed = new Dictionary<string, object>();
			if (_StatusEnumChanged)
				changed.Add("status_enum", _StatusEnum);
			if (_LastUpdatedChanged)
				changed.Add("last_updated", _LastUpdated);
			if (_TotalVolumesReleasedChanged)
				changed.Add("total_volumes_released", _TotalVolumesReleased);
			if (_MangaUpdatesIdChanged)
				changed.Add("manga_updates_id", _MangaUpdatesId);
			if (_YearReleasedChanged)
				changed.Add("year_released", _YearReleased);
			if (_DescriptionChanged)
				changed.Add("description", _Description);

			return changed;
		}

		public override object[] getAllValues() {
			return new object[] {
				_StatusEnum,
				_LastUpdated,
				_TotalVolumesReleased,
				_MangaUpdatesId,
				_YearReleased,
				_Description,
			};
		}

		public override string[] getColumns() {
			return new string[] {
				"status_enum",
				"last_updated",
				"total_volumes_released",
				"manga_updates_id",
				"year_released",
				"description",
			};
		}

		public override string getPKName() {
			return "rowid";
		}

		public override object getPKValue() {
			return _rowid;
		}

	}

	[TableAttribute(Name = "Tags")]
	public class Tags : Model {
		private System.Int32 _rowid;
		public System.Int32 rowid {
			get { return _rowid; }
		}

		private bool _ValueChanged = false;
		private System.String _Value;
		public System.String Value {
			get { return _Value; }
			set {
				_Value = value;
				_ValueChanged = true;
			}
		}

		private PageTags _PageTags;
		public PageTags PageTags {
			get {
				if(_PageTags == null){ 
					try {
						_PageTags = ((TomeContext)context).PageTags.select().whereIn("Tags_rowid", _rowid).executeFetch();
					} catch {
						//Accessing a property outside of its database context is not allowed.  Access an association inside the database context to cache the values for later use.
						_PageTags = null;
					}
				}
				return _PageTags;
			}
		}

		public Tags() : this(null, null) { }

		public Tags(DbDataReader reader, Context context) {
			read(reader, context);
		}

		public override void read(DbDataReader reader, Context context) {
			this.context = context;
			if (reader == null) { return; }

			int length = reader.FieldCount;
			for (int i = 0; i < length; i++) {
				switch (reader.GetName(i)) {
					case "rowid": _rowid = (reader.IsDBNull(i)) ? default(System.Int32) : reader.GetInt32(i); break;
					case "value": _Value = reader.GetValue(i) as System.String; break;
					default: break;
				}
			}
		}

		public override Dictionary<string, object> getChangedValues() {
			var changed = new Dictionary<string, object>();
			if (_ValueChanged)
				changed.Add("value", _Value);

			return changed;
		}

		public override object[] getAllValues() {
			return new object[] {
				_Value,
			};
		}

		public override string[] getColumns() {
			return new string[] {
				"value",
			};
		}

		public override string getPKName() {
			return "rowid";
		}

		public override object getPKValue() {
			return _rowid;
		}

	}

	[TableAttribute(Name = "PageTags")]
	public class PageTags : Model {
		private System.Int32 _rowid;
		public System.Int32 rowid {
			get { return _rowid; }
		}

		private bool _PagesRowidChanged = false;
		private System.Int32 _PagesRowid;
		public System.Int32 PagesRowid {
			get { return _PagesRowid; }
			set {
				_PagesRowid = value;
				_PagesRowidChanged = true;
			}
		}

		private bool _TagsRowidChanged = false;
		private System.Int32 _TagsRowid;
		public System.Int32 TagsRowid {
			get { return _TagsRowid; }
			set {
				_TagsRowid = value;
				_TagsRowidChanged = true;
			}
		}

		private Tags _Tags;
		public Tags Tags {
			get {
				if(_Tags == null){ 
					try {
						_Tags = ((TomeContext)context).Tags.select().whereIn("rowid", _TagsRowid).executeFetch();
					} catch {
						//Accessing a property outside of its database context is not allowed.  Access an association inside the database context to cache the values for later use.
						_Tags = null;
					}
				}
				return _Tags;
			}
		}

		private Pages[] _Pages;
		public Pages[] Pages {
			get {
				if(_Pages == null){ 
					try {
						_Pages = ((TomeContext)context).Pages.select().whereIn("rowid", _PagesRowid).executeFetchAll();
					} catch {
						//Accessing a property outside of its database context is not allowed.  Access an association inside the database context to cache the values for later use.
						_Pages = null;
					}
				}
				return _Pages;
			}
		}

		public PageTags() : this(null, null) { }

		public PageTags(DbDataReader reader, Context context) {
			read(reader, context);
		}

		public override void read(DbDataReader reader, Context context) {
			this.context = context;
			if (reader == null) { return; }

			int length = reader.FieldCount;
			for (int i = 0; i < length; i++) {
				switch (reader.GetName(i)) {
					case "rowid": _rowid = (reader.IsDBNull(i)) ? default(System.Int32) : reader.GetInt32(i); break;
					case "Pages_rowid": _PagesRowid = (reader.IsDBNull(i)) ? default(System.Int32) : reader.GetInt32(i); break;
					case "Tags_rowid": _TagsRowid = (reader.IsDBNull(i)) ? default(System.Int32) : reader.GetInt32(i); break;
					default: break;
				}
			}
		}

		public override Dictionary<string, object> getChangedValues() {
			var changed = new Dictionary<string, object>();
			if (_PagesRowidChanged)
				changed.Add("Pages_rowid", _PagesRowid);
			if (_TagsRowidChanged)
				changed.Add("Tags_rowid", _TagsRowid);

			return changed;
		}

		public override object[] getAllValues() {
			return new object[] {
				_PagesRowid,
				_TagsRowid,
			};
		}

		public override string[] getColumns() {
			return new string[] {
				"Pages_rowid",
				"Tags_rowid",
			};
		}

		public override string getPKName() {
			return "rowid";
		}

		public override object getPKValue() {
			return _rowid;
		}

	}

	[TableAttribute(Name = "Pages")]
	public class Pages : Model {
		private System.Int32 _rowid;
		public System.Int32 rowid {
			get { return _rowid; }
		}

		private bool _Files_rowidChanged = false;
		private System.Int32 _Files_rowid;
		public System.Int32 Files_rowid {
			get { return _Files_rowid; }
			set {
				_Files_rowid = value;
				_Files_rowidChanged = true;
			}
		}

		private bool _Volumes_rowidChanged = false;
		private System.Int32 _Volumes_rowid;
		public System.Int32 Volumes_rowid {
			get { return _Volumes_rowid; }
			set {
				_Volumes_rowid = value;
				_Volumes_rowidChanged = true;
			}
		}

		private PageTags _PageTags;
		public PageTags PageTags {
			get {
				if(_PageTags == null){ 
					try {
						_PageTags = ((TomeContext)context).PageTags.select().whereIn("Pages_rowid", _rowid).executeFetch();
					} catch {
						//Accessing a property outside of its database context is not allowed.  Access an association inside the database context to cache the values for later use.
						_PageTags = null;
					}
				}
				return _PageTags;
			}
		}

		private Volumes[] _Volume;
		public Volumes[] Volume {
			get {
				if(_Volume == null){ 
					try {
						_Volume = ((TomeContext)context).Volumes.select().whereIn("rowid", _Volumes_rowid).executeFetchAll();
					} catch {
						//Accessing a property outside of its database context is not allowed.  Access an association inside the database context to cache the values for later use.
						_Volume = null;
					}
				}
				return _Volume;
			}
		}

		public Pages() : this(null, null) { }

		public Pages(DbDataReader reader, Context context) {
			read(reader, context);
		}

		public override void read(DbDataReader reader, Context context) {
			this.context = context;
			if (reader == null) { return; }

			int length = reader.FieldCount;
			for (int i = 0; i < length; i++) {
				switch (reader.GetName(i)) {
					case "rowid": _rowid = (reader.IsDBNull(i)) ? default(System.Int32) : reader.GetInt32(i); break;
					case "Files_rowid": _Files_rowid = (reader.IsDBNull(i)) ? default(System.Int32) : reader.GetInt32(i); break;
					case "Volumes_rowid": _Volumes_rowid = (reader.IsDBNull(i)) ? default(System.Int32) : reader.GetInt32(i); break;
					default: break;
				}
			}
		}

		public override Dictionary<string, object> getChangedValues() {
			var changed = new Dictionary<string, object>();
			if (_Files_rowidChanged)
				changed.Add("Files_rowid", _Files_rowid);
			if (_Volumes_rowidChanged)
				changed.Add("Volumes_rowid", _Volumes_rowid);

			return changed;
		}

		public override object[] getAllValues() {
			return new object[] {
				_Files_rowid,
				_Volumes_rowid,
			};
		}

		public override string[] getColumns() {
			return new string[] {
				"Files_rowid",
				"Volumes_rowid",
			};
		}

		public override string getPKName() {
			return "rowid";
		}

		public override object getPKValue() {
			return _rowid;
		}

	}

	[TableAttribute(Name = "Genres")]
	public class Genres : Model {
		private System.Int32 _rowid;
		public System.Int32 rowid {
			get { return _rowid; }
		}

		private bool _valueChanged = false;
		private System.String _value;
		public System.String value {
			get { return _value; }
			set {
				_value = value;
				_valueChanged = true;
			}
		}

		private MangaGenres[] _MangaGenres;
		public MangaGenres[] MangaGenres {
			get {
				if(_MangaGenres == null){ 
					try {
						_MangaGenres = ((TomeContext)context).MangaGenres.select().whereIn("Genres_rowid", _rowid).executeFetchAll();
					} catch {
						//Accessing a property outside of its database context is not allowed.  Access an association inside the database context to cache the values for later use.
						_MangaGenres = null;
					}
				}
				return _MangaGenres;
			}
		}

		public Genres() : this(null, null) { }

		public Genres(DbDataReader reader, Context context) {
			read(reader, context);
		}

		public override void read(DbDataReader reader, Context context) {
			this.context = context;
			if (reader == null) { return; }

			int length = reader.FieldCount;
			for (int i = 0; i < length; i++) {
				switch (reader.GetName(i)) {
					case "rowid": _rowid = (reader.IsDBNull(i)) ? default(System.Int32) : reader.GetInt32(i); break;
					case "value": _value = reader.GetValue(i) as System.String; break;
					default: break;
				}
			}
		}

		public override Dictionary<string, object> getChangedValues() {
			var changed = new Dictionary<string, object>();
			if (_valueChanged)
				changed.Add("value", _value);

			return changed;
		}

		public override object[] getAllValues() {
			return new object[] {
				_value,
			};
		}

		public override string[] getColumns() {
			return new string[] {
				"value",
			};
		}

		public override string getPKName() {
			return "rowid";
		}

		public override object getPKValue() {
			return _rowid;
		}

	}

	[TableAttribute(Name = "MangaGenres")]
	public class MangaGenres : Model {
		private System.Int32 _rowid;
		public System.Int32 rowid {
			get { return _rowid; }
		}

		private bool _MangaRowidChanged = false;
		private System.Int32 _MangaRowid;
		public System.Int32 MangaRowid {
			get { return _MangaRowid; }
			set {
				_MangaRowid = value;
				_MangaRowidChanged = true;
			}
		}

		private bool _GenresRowidChanged = false;
		private System.Int32 _GenresRowid;
		public System.Int32 GenresRowid {
			get { return _GenresRowid; }
			set {
				_GenresRowid = value;
				_GenresRowidChanged = true;
			}
		}

		private Genres _Genres;
		public Genres Genres {
			get {
				if(_Genres == null){ 
					try {
						_Genres = ((TomeContext)context).Genres.select().whereIn("rowid", _GenresRowid).executeFetch();
					} catch {
						//Accessing a property outside of its database context is not allowed.  Access an association inside the database context to cache the values for later use.
						_Genres = null;
					}
				}
				return _Genres;
			}
		}

		private Manga[] _Manga;
		public Manga[] Manga {
			get {
				if(_Manga == null){ 
					try {
						_Manga = ((TomeContext)context).Manga.select().whereIn("rowid", _MangaRowid).executeFetchAll();
					} catch {
						//Accessing a property outside of its database context is not allowed.  Access an association inside the database context to cache the values for later use.
						_Manga = null;
					}
				}
				return _Manga;
			}
		}

		public MangaGenres() : this(null, null) { }

		public MangaGenres(DbDataReader reader, Context context) {
			read(reader, context);
		}

		public override void read(DbDataReader reader, Context context) {
			this.context = context;
			if (reader == null) { return; }

			int length = reader.FieldCount;
			for (int i = 0; i < length; i++) {
				switch (reader.GetName(i)) {
					case "rowid": _rowid = (reader.IsDBNull(i)) ? default(System.Int32) : reader.GetInt32(i); break;
					case "Manga_rowid": _MangaRowid = (reader.IsDBNull(i)) ? default(System.Int32) : reader.GetInt32(i); break;
					case "Genres_rowid": _GenresRowid = (reader.IsDBNull(i)) ? default(System.Int32) : reader.GetInt32(i); break;
					default: break;
				}
			}
		}

		public override Dictionary<string, object> getChangedValues() {
			var changed = new Dictionary<string, object>();
			if (_MangaRowidChanged)
				changed.Add("Manga_rowid", _MangaRowid);
			if (_GenresRowidChanged)
				changed.Add("Genres_rowid", _GenresRowid);

			return changed;
		}

		public override object[] getAllValues() {
			return new object[] {
				_MangaRowid,
				_GenresRowid,
			};
		}

		public override string[] getColumns() {
			return new string[] {
				"Manga_rowid",
				"Genres_rowid",
			};
		}

		public override string getPKName() {
			return "rowid";
		}

		public override object getPKValue() {
			return _rowid;
		}

	}

	[TableAttribute(Name = "Categories")]
	public class Categories : Model {
		private System.Int32 _rowid;
		public System.Int32 rowid {
			get { return _rowid; }
		}

		private bool _valueChanged = false;
		private System.String _value;
		public System.String value {
			get { return _value; }
			set {
				_value = value;
				_valueChanged = true;
			}
		}

		private MangaCategories[] _MangaCategories;
		public MangaCategories[] MangaCategories {
			get {
				if(_MangaCategories == null){ 
					try {
						_MangaCategories = ((TomeContext)context).MangaCategories.select().whereIn("Categories_rowid", _rowid).executeFetchAll();
					} catch {
						//Accessing a property outside of its database context is not allowed.  Access an association inside the database context to cache the values for later use.
						_MangaCategories = null;
					}
				}
				return _MangaCategories;
			}
		}

		public Categories() : this(null, null) { }

		public Categories(DbDataReader reader, Context context) {
			read(reader, context);
		}

		public override void read(DbDataReader reader, Context context) {
			this.context = context;
			if (reader == null) { return; }

			int length = reader.FieldCount;
			for (int i = 0; i < length; i++) {
				switch (reader.GetName(i)) {
					case "rowid": _rowid = (reader.IsDBNull(i)) ? default(System.Int32) : reader.GetInt32(i); break;
					case "value": _value = reader.GetValue(i) as System.String; break;
					default: break;
				}
			}
		}

		public override Dictionary<string, object> getChangedValues() {
			var changed = new Dictionary<string, object>();
			if (_valueChanged)
				changed.Add("value", _value);

			return changed;
		}

		public override object[] getAllValues() {
			return new object[] {
				_value,
			};
		}

		public override string[] getColumns() {
			return new string[] {
				"value",
			};
		}

		public override string getPKName() {
			return "rowid";
		}

		public override object getPKValue() {
			return _rowid;
		}

	}

	[TableAttribute(Name = "MangaCategories")]
	public class MangaCategories : Model {
		private System.Int32 _rowid;
		public System.Int32 rowid {
			get { return _rowid; }
		}

		private bool _MangaRowidChanged = false;
		private System.Int32 _MangaRowid;
		public System.Int32 MangaRowid {
			get { return _MangaRowid; }
			set {
				_MangaRowid = value;
				_MangaRowidChanged = true;
			}
		}

		private bool _CategoriesRowidChanged = false;
		private System.Int32 _CategoriesRowid;
		public System.Int32 CategoriesRowid {
			get { return _CategoriesRowid; }
			set {
				_CategoriesRowid = value;
				_CategoriesRowidChanged = true;
			}
		}

		private Categories _Categories;
		public Categories Categories {
			get {
				if(_Categories == null){ 
					try {
						_Categories = ((TomeContext)context).Categories.select().whereIn("rowid", _CategoriesRowid).executeFetch();
					} catch {
						//Accessing a property outside of its database context is not allowed.  Access an association inside the database context to cache the values for later use.
						_Categories = null;
					}
				}
				return _Categories;
			}
		}

		private Manga[] _Manga;
		public Manga[] Manga {
			get {
				if(_Manga == null){ 
					try {
						_Manga = ((TomeContext)context).Manga.select().whereIn("rowid", _MangaRowid).executeFetchAll();
					} catch {
						//Accessing a property outside of its database context is not allowed.  Access an association inside the database context to cache the values for later use.
						_Manga = null;
					}
				}
				return _Manga;
			}
		}

		public MangaCategories() : this(null, null) { }

		public MangaCategories(DbDataReader reader, Context context) {
			read(reader, context);
		}

		public override void read(DbDataReader reader, Context context) {
			this.context = context;
			if (reader == null) { return; }

			int length = reader.FieldCount;
			for (int i = 0; i < length; i++) {
				switch (reader.GetName(i)) {
					case "rowid": _rowid = (reader.IsDBNull(i)) ? default(System.Int32) : reader.GetInt32(i); break;
					case "Manga_rowid": _MangaRowid = (reader.IsDBNull(i)) ? default(System.Int32) : reader.GetInt32(i); break;
					case "Categories_rowid": _CategoriesRowid = (reader.IsDBNull(i)) ? default(System.Int32) : reader.GetInt32(i); break;
					default: break;
				}
			}
		}

		public override Dictionary<string, object> getChangedValues() {
			var changed = new Dictionary<string, object>();
			if (_MangaRowidChanged)
				changed.Add("Manga_rowid", _MangaRowid);
			if (_CategoriesRowidChanged)
				changed.Add("Categories_rowid", _CategoriesRowid);

			return changed;
		}

		public override object[] getAllValues() {
			return new object[] {
				_MangaRowid,
				_CategoriesRowid,
			};
		}

		public override string[] getColumns() {
			return new string[] {
				"Manga_rowid",
				"Categories_rowid",
			};
		}

		public override string getPKName() {
			return "rowid";
		}

		public override object getPKValue() {
			return _rowid;
		}

	}

	[TableAttribute(Name = "People")]
	public class People : Model {
		private System.Int32 _rowid;
		public System.Int32 rowid {
			get { return _rowid; }
		}

		private bool _NameChanged = false;
		private System.String _Name;
		public System.String Name {
			get { return _Name; }
			set {
				_Name = value;
				_NameChanged = true;
			}
		}

		private MangaPeople[] _MangaPeople;
		public MangaPeople[] MangaPeople {
			get {
				if(_MangaPeople == null){ 
					try {
						_MangaPeople = ((TomeContext)context).MangaPeople.select().whereIn("People_rowid", _rowid).executeFetchAll();
					} catch {
						//Accessing a property outside of its database context is not allowed.  Access an association inside the database context to cache the values for later use.
						_MangaPeople = null;
					}
				}
				return _MangaPeople;
			}
		}

		public People() : this(null, null) { }

		public People(DbDataReader reader, Context context) {
			read(reader, context);
		}

		public override void read(DbDataReader reader, Context context) {
			this.context = context;
			if (reader == null) { return; }

			int length = reader.FieldCount;
			for (int i = 0; i < length; i++) {
				switch (reader.GetName(i)) {
					case "rowid": _rowid = (reader.IsDBNull(i)) ? default(System.Int32) : reader.GetInt32(i); break;
					case "first_name": _Name = reader.GetValue(i) as System.String; break;
					default: break;
				}
			}
		}

		public override Dictionary<string, object> getChangedValues() {
			var changed = new Dictionary<string, object>();
			if (_NameChanged)
				changed.Add("first_name", _Name);

			return changed;
		}

		public override object[] getAllValues() {
			return new object[] {
				_Name,
			};
		}

		public override string[] getColumns() {
			return new string[] {
				"first_name",
			};
		}

		public override string getPKName() {
			return "rowid";
		}

		public override object getPKValue() {
			return _rowid;
		}

	}

	[TableAttribute(Name = "MangaPeople")]
	public class MangaPeople : Model {
		private System.Int32 _rowid;
		public System.Int32 rowid {
			get { return _rowid; }
		}

		private bool _PeopleRowidChanged = false;
		private System.Int32 _PeopleRowid;
		public System.Int32 PeopleRowid {
			get { return _PeopleRowid; }
			set {
				_PeopleRowid = value;
				_PeopleRowidChanged = true;
			}
		}

		private bool _MangaRowidChanged = false;
		private System.Int32 _MangaRowid;
		public System.Int32 MangaRowid {
			get { return _MangaRowid; }
			set {
				_MangaRowid = value;
				_MangaRowidChanged = true;
			}
		}

		private bool _IsArtistChanged = false;
		private System.Boolean _IsArtist;
		public System.Boolean IsArtist {
			get { return _IsArtist; }
			set {
				_IsArtist = value;
				_IsArtistChanged = true;
			}
		}

		private bool _IsAuthorChanged = false;
		private System.Boolean _IsAuthor;
		public System.Boolean IsAuthor {
			get { return _IsAuthor; }
			set {
				_IsAuthor = value;
				_IsAuthorChanged = true;
			}
		}

		private People _People;
		public People People {
			get {
				if(_People == null){ 
					try {
						_People = ((TomeContext)context).People.select().whereIn("rowid", _PeopleRowid).executeFetch();
					} catch {
						//Accessing a property outside of its database context is not allowed.  Access an association inside the database context to cache the values for later use.
						_People = null;
					}
				}
				return _People;
			}
		}

		private Manga[] _Manga;
		public Manga[] Manga {
			get {
				if(_Manga == null){ 
					try {
						_Manga = ((TomeContext)context).Manga.select().whereIn("rowid", _MangaRowid).executeFetchAll();
					} catch {
						//Accessing a property outside of its database context is not allowed.  Access an association inside the database context to cache the values for later use.
						_Manga = null;
					}
				}
				return _Manga;
			}
		}

		public MangaPeople() : this(null, null) { }

		public MangaPeople(DbDataReader reader, Context context) {
			read(reader, context);
		}

		public override void read(DbDataReader reader, Context context) {
			this.context = context;
			if (reader == null) { return; }

			int length = reader.FieldCount;
			for (int i = 0; i < length; i++) {
				switch (reader.GetName(i)) {
					case "rowid": _rowid = (reader.IsDBNull(i)) ? default(System.Int32) : reader.GetInt32(i); break;
					case "People_rowid": _PeopleRowid = (reader.IsDBNull(i)) ? default(System.Int32) : reader.GetInt32(i); break;
					case "Manga_rowid": _MangaRowid = (reader.IsDBNull(i)) ? default(System.Int32) : reader.GetInt32(i); break;
					case "is_artist": _IsArtist = (reader.IsDBNull(i)) ? default(System.Boolean) : reader.GetBoolean(i); break;
					case "is_author": _IsAuthor = (reader.IsDBNull(i)) ? default(System.Boolean) : reader.GetBoolean(i); break;
					default: break;
				}
			}
		}

		public override Dictionary<string, object> getChangedValues() {
			var changed = new Dictionary<string, object>();
			if (_PeopleRowidChanged)
				changed.Add("People_rowid", _PeopleRowid);
			if (_MangaRowidChanged)
				changed.Add("Manga_rowid", _MangaRowid);
			if (_IsArtistChanged)
				changed.Add("is_artist", _IsArtist);
			if (_IsAuthorChanged)
				changed.Add("is_author", _IsAuthor);

			return changed;
		}

		public override object[] getAllValues() {
			return new object[] {
				_PeopleRowid,
				_MangaRowid,
				_IsArtist,
				_IsAuthor,
			};
		}

		public override string[] getColumns() {
			return new string[] {
				"People_rowid",
				"Manga_rowid",
				"is_artist",
				"is_author",
			};
		}

		public override string getPKName() {
			return "rowid";
		}

		public override object getPKValue() {
			return _rowid;
		}

	}

	[TableAttribute(Name = "MangaTitles")]
	public class MangaTitles : Model {
		private System.Int32 _rowid;
		public System.Int32 rowid {
			get { return _rowid; }
		}

		private bool _MangaRowidChanged = false;
		private System.Int32 _MangaRowid;
		public System.Int32 MangaRowid {
			get { return _MangaRowid; }
			set {
				_MangaRowid = value;
				_MangaRowidChanged = true;
			}
		}

		private bool _ValueChanged = false;
		private System.String _Value;
		public System.String Value {
			get { return _Value; }
			set {
				_Value = value;
				_ValueChanged = true;
			}
		}

		private bool _IsPrimaryChanged = false;
		private System.Boolean _IsPrimary;
		public System.Boolean IsPrimary {
			get { return _IsPrimary; }
			set {
				_IsPrimary = value;
				_IsPrimaryChanged = true;
			}
		}

		private Manga _Manga;
		public Manga Manga {
			get {
				if(_Manga == null){ 
					try {
						_Manga = ((TomeContext)context).Manga.select().whereIn("rowid", _MangaRowid).executeFetch();
					} catch {
						//Accessing a property outside of its database context is not allowed.  Access an association inside the database context to cache the values for later use.
						_Manga = null;
					}
				}
				return _Manga;
			}
		}

		public MangaTitles() : this(null, null) { }

		public MangaTitles(DbDataReader reader, Context context) {
			read(reader, context);
		}

		public override void read(DbDataReader reader, Context context) {
			this.context = context;
			if (reader == null) { return; }

			int length = reader.FieldCount;
			for (int i = 0; i < length; i++) {
				switch (reader.GetName(i)) {
					case "rowid": _rowid = (reader.IsDBNull(i)) ? default(System.Int32) : reader.GetInt32(i); break;
					case "Manga_rowid": _MangaRowid = (reader.IsDBNull(i)) ? default(System.Int32) : reader.GetInt32(i); break;
					case "name": _Value = reader.GetValue(i) as System.String; break;
					case "is_primary": _IsPrimary = (reader.IsDBNull(i)) ? default(System.Boolean) : reader.GetBoolean(i); break;
					default: break;
				}
			}
		}

		public override Dictionary<string, object> getChangedValues() {
			var changed = new Dictionary<string, object>();
			if (_MangaRowidChanged)
				changed.Add("Manga_rowid", _MangaRowid);
			if (_ValueChanged)
				changed.Add("name", _Value);
			if (_IsPrimaryChanged)
				changed.Add("is_primary", _IsPrimary);

			return changed;
		}

		public override object[] getAllValues() {
			return new object[] {
				_MangaRowid,
				_Value,
				_IsPrimary,
			};
		}

		public override string[] getColumns() {
			return new string[] {
				"Manga_rowid",
				"name",
				"is_primary",
			};
		}

		public override string getPKName() {
			return "rowid";
		}

		public override object getPKValue() {
			return _rowid;
		}

	}
}