var postsController = function (dataService) {

  function publish(id) {
    dataService.put("admin/publishpost/" + id + "?flag=P", null, callback, fail);
  }

  function unpublish(id) {
    dataService.put("admin/publishpost/" + id + "?flag=U", null, callback, fail);
  }

  function feature(id) {
      dataService.put("admin/featurepost/" + id + "?flag=F", null, callback, fail);
  }

  function unfeature(id) {
      dataService.put("admin/featurepost/" + id + "?flag=U", null, callback, fail);
  }

  function remove(id) {
      dataService.remove("admin/removepost/" + id, callback, fail);
  }

    $('#addComment').on('click', function () {
        var comment = $('#text1').val();
        if (comment.length === 0) {
            $('#text1').addClass("border border-danger");
        }
        else {
            var postId = $("#postId").val();
            dataService.post('post/addcomment/' + postId + "/" + comment, null, function (data) {
                $('#comments').html(data);
            }, fail);
        }
    });



  function callback() {
    toastr.success('Updated');
    setTimeout(function () {
        window.location.href = webRoot + 'admin';
    }, 1000);
  }

  function filter() {
    var status = $('input[name=status-filter]:checked').val();
    var url = webRoot + "admin/posts?status=" + status;
    window.location.href = url;
  }

  return {
    publish: publish,
    unpublish: unpublish,
    feature: feature,
    unfeature: unfeature,
    remove: remove,
    filter: filter
  };
}(DataService);

// filters collapsable
$('.filter-group-title').on('click', function() {
  if ($(window).width() < 992) {
    $(this).toggleClass('active');
    $(this).parent().toggleClass('active');
  }
});

