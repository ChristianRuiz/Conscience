import React from 'react';
import CircularProgress from 'material-ui/CircularProgress';
import FileUpload from 'react-fileupload';

class BulkImport extends React.Component {

  constructor(props) {
    super(props);

    this.state = {
      isUploading: false,
      progress: 0,
      uploaded: false,
      hasError: false,
      uploadSuccesses: [],
      uploadErrors: []
    };
  }

  render() {
    const self = this;

    const options = {
      baseUrl: '/api/BulkImport',
      param: {
        _: new Date().getTime()
      },
      chooseAndUpload: true,
      withCredentials: true,
      beforeUpload() {
        self.setState({ hasError: false, isUploading: true, progress: 0, uploaded: false });
      },
      uploading(progress) {
        self.setState({ progress: (progress.loaded / progress.total) });
      },
      uploadSuccess(result) {
        self.setState({ hasError: false, isUploading: false, uploaded: true, uploadSuccesses: result.Successes, uploadErrors: result.Errors });
      },
      uploadError(error) {
        self.setState({ hasError: true, isUploading: false, uploaded: false });
      },
      uploadFail() {
        self.setState({ hasError: true, isUploading: false, uploaded: false });
      }
    };

    if (this.state.isUploading) {
      return <CircularProgress size={80} thickness={5} />;
    }

    return (<div>
      <FileUpload options={options}>
        <button ref="chooseAndUpload">Upload Excel</button>
      </FileUpload>
      {this.state.hasError &&
        <h3>Upload error</h3>
                    }
      {this.state.uploaded &&
        <div>
          <p>Successes: {this.state.uploadSuccesses.length}</p>

          <ul>
            {this.state.uploadErrors
          .map(error =>
            <li key={error.Hash}><b>{error.Line}: </b> {error.Error}</li>)
            }
          </ul>
        </div>
                    }
    </div>);
  }
}

export default BulkImport;
