const AddStudent = async (student) => {
    try { 
        const result = await AjaxPOST('/Student/AddStudent', student);
        if (result.success) {
            alert('Student added successfully');
            window.location.href = '/Student/Index';
        } else {
            alert('Failed to add student: ' + result.message);
        }
    } catch (e) {
        console.error('Error adding student:', e);
        alert("An error occured while adding the student.");
    }
};

$(document).ready(function () {
    $('#addStudentForm').on('submit', function (e) {
        e.preventDefault();
        const student = {
            firstName: $('#firstName').val(),
            lastName: $('#lastName').val(),
            Age: $('#Age').val(),
            Course: $('#Course').val()
        };
        AddStudent(student);
    }); 
});

const getStudentById = async (id) => {
    try {
        return await AjaxGET(`/Student/GetStudentById/${id}`);

    }
    catch (e) {
        console.error('Error getting student:', e);
        alert("An error occured while getting the student.");
    }
};